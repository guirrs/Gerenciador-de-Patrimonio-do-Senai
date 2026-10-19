using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.DTOs.SolicitacaoTransferenciaDto;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;

namespace GestaoPatrimonios.Applications.Services
{
    public class SolicitacaoTransferenciaService
    {
        // Constantes evitam "magic strings" espalhadas e erros de digitação
        private static class TipoUsuarioNome
        {
            public const string Responsavel = "Responsável";
        }

        private static class StatusTransferenciaNome
        {
            public const string Pendente = "Pendente de aprovação";
            public const string Aprovado = "Aprovado";
            public const string Recusado = "Recusado";
        }

        private const string StatusPatrimonioTransferido = "Transferido";
        private const string TipoAlteracaoTransferencia = "Transferência";

        private readonly ISolicitacaoTransferenciaRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public SolicitacaoTransferenciaService(
            ISolicitacaoTransferenciaRepository repository,
            IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public List<ListarSolicitacaoTransferenciaDto> Listar()
        {
            return _repository.Listar()
                .Select(MapearParaDto)
                .ToList();
        }

        public ListarSolicitacaoTransferenciaDto BuscarPorId(Guid transferenciaId)
        {
            SolicitacaoTransferencia solicitacao = ObterSolicitacao(transferenciaId);

            return MapearParaDto(solicitacao);
        }

        public void Adicionar(Guid usuarioId, CriarSolicitacaoTransferenciaDto dto)
        {
            Validar.ValidarJustificativa(dto.Justificativa);

            Usuario usuario = ObterUsuario(usuarioId);
            Patrimonio patrimonio = ObterPatrimonio(dto.PatrimonioID);

            if (!_repository.LocalizacaoExiste(dto.LocalizacaoID))
                throw new DomainException("Localização de destino não existe.");

            if (patrimonio.LocalizacaoID == dto.LocalizacaoID)
                throw new DomainException("O patrimônio já está nessa localização.");

            if (_repository.ExisteSolicitacaoPendente(dto.PatrimonioID))
                throw new DomainException("Já existe uma solicitação pendente para esse patrimônio.");

            if (EhResponsavel(usuario) &&
                !_repository.UsuarioResponsavelDaLocalizacao(usuarioId, patrimonio.LocalizacaoID))
            {
                throw new DomainException(
                    "O responsável só pode solicitar transferência de patrimônio do ambiente ao qual está vinculado.");
            }

            StatusTransferencia statusPendente = ObterStatusTransferencia(StatusTransferenciaNome.Pendente);

            SolicitacaoTransferencia solicitacao = new SolicitacaoTransferencia
            {
                DataCriacaoSolicitante = DateTime.Now,
                Justificativa = dto.Justificativa,
                StatusTransferenciaID = statusPendente.StatusTransferenciaID,
                UsuarioIDSolicitacao = usuarioId,
                UsuarioIDAprovacao = null,
                PatrimonioID = dto.PatrimonioID,
                LocalizacaoID = dto.LocalizacaoID
            };

            _repository.Adicionar(solicitacao);
        }

        public void Responder(Guid transferenciaId, Guid usuarioId, ResponderSolicitacaoTransferenciaDto dto)
        {
            // ---------- 1. Carrega e valida TUDO antes de gravar qualquer coisa ----------
            Usuario usuario = ObterUsuario(usuarioId);
            SolicitacaoTransferencia solicitacao = ObterSolicitacao(transferenciaId);
            Patrimonio patrimonio = ObterPatrimonio(solicitacao.PatrimonioID);

            StatusTransferencia statusPendente = ObterStatusTransferencia(StatusTransferenciaNome.Pendente);

            if (solicitacao.StatusTransferenciaID != statusPendente.StatusTransferenciaID)
                throw new DomainException("Essa solicitação já foi respondida.");

            if (EhResponsavel(usuario) &&
                !_repository.UsuarioResponsavelDaLocalizacao(usuarioId, patrimonio.LocalizacaoID))
            {
                throw new DomainException(
                    "Somente o responsável do ambiente de origem pode aprovar ou rejeitar essa solicitação.");
            }

            StatusTransferencia statusResposta = ObterStatusTransferencia(
                dto.Aprovado ? StatusTransferenciaNome.Aprovado : StatusTransferenciaNome.Recusado);

            // Só busca os dados de patrimônio/log se for aprovação
            StatusPatrimonio? statusTransferido = null;
            TipoAlteracao? tipoAlteracao = null;

            if (dto.Aprovado)
            {
                statusTransferido = _repository.BuscarStatusPatrimonioPorNome(StatusPatrimonioTransferido)
                    ?? throw new DomainException("Status de patrimônio 'Transferido' não encontrado.");

                tipoAlteracao = _repository.BuscarTipoAlteracaoPorNome(TipoAlteracaoTransferencia)
                    ?? throw new DomainException("Tipo alteração 'Transferência' não encontrado.");
            }

            // ---------- 2. Só agora aplica as alterações ----------
            solicitacao.StatusTransferenciaID = statusResposta.StatusTransferenciaID;
            solicitacao.UsuarioIDAprovacao = usuarioId;
            solicitacao.DataResposta = DateTime.Now;

            _repository.Atualizar(solicitacao);

            if (!dto.Aprovado)
                return;

            patrimonio.LocalizacaoID = solicitacao.LocalizacaoID;
            patrimonio.StatusPatrimonioID = statusTransferido!.StatusPatrimonioID;

            _repository.AtualizarPatrimonio(patrimonio);

            _repository.AdicionarLog(new LogPatrimonio
            {
                DataTransferencia = DateTime.Now,
                TipoAlteracaoID = tipoAlteracao!.TipoAlteracaoID,
                StatusPatrimonioID = statusTransferido.StatusPatrimonioID,
                PatrimonioID = patrimonio.PatrimonioID,
                UsuarioID = usuarioId,
                LocalizacaoID = patrimonio.LocalizacaoID
            });
        }

        // ======================= Métodos auxiliares =======================

        private static ListarSolicitacaoTransferenciaDto MapearParaDto(SolicitacaoTransferencia solicitacao)
        {
            return new ListarSolicitacaoTransferenciaDto
            {
                TransferenciaID = solicitacao.TransferenciaID,
                DataCriacaoSolicitante = solicitacao.DataCriacaoSolicitante,
                DataResposta = solicitacao.DataResposta,
                Justificativa = solicitacao.Justificativa,
                StatusTransferenciaID = solicitacao.StatusTransferenciaID,
                UsuarioIDSolicitacao = solicitacao.UsuarioIDSolicitacao,
                UsuarioIDAprovacao = solicitacao.UsuarioIDAprovacao,
                PatrimonioID = solicitacao.PatrimonioID,
                LocalizacaoID = solicitacao.LocalizacaoID
            };
        }

        private static bool EhResponsavel(Usuario usuario)
        {
            return usuario.TipoUsuario.NomeTipo == TipoUsuarioNome.Responsavel;
        }

        private Usuario ObterUsuario(Guid usuarioId)
        {
            return _usuarioRepository.BuscarPorId(usuarioId)
                ?? throw new DomainException("Usuário não encontrado.");
        }

        private SolicitacaoTransferencia ObterSolicitacao(Guid transferenciaId)
        {
            return _repository.BuscarPorId(transferenciaId)
                ?? throw new DomainException("Solicitação de transferência não encontrada.");
        }

        private Patrimonio ObterPatrimonio(Guid patrimonioId)
        {
            return _repository.BuscarPatrimonioPorId(patrimonioId)
                ?? throw new DomainException("Patrimônio não encontrado.");
        }

        private StatusTransferencia ObterStatusTransferencia(string nome)
        {
            return _repository.BuscarStatusTransferenciaPorNome(nome)
                ?? throw new DomainException($"Status de transferência '{nome}' não encontrado.");
        }
    }
}