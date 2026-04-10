using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Aplication.Regras;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.SolicitacaoTransferenciaDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Aplication.Services
{
    public class SolicitacaoTransferenciaService
    {
        private readonly ISolicitacaoTransferenciaRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public SolicitacaoTransferenciaService(ISolicitacaoTransferenciaRepository repository, IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public List<ListarSolicitacaoTransferenciaDto> Listar()
        {
            return _repository.Listar().Select(s => SolicitacaoTransferenciaParaDto.ConverterParaDto(s)).ToList();
        }

        public ListarSolicitacaoTransferenciaDto ObterPorId(Guid id)
        {
            SolicitacaoTransferencia solicitacao = _repository.ObterPorId(id);

            if (solicitacao == null)
                throw new DomainException("Solicitacao de tranferencia não encontrada.");

            return SolicitacaoTransferenciaParaDto.ConverterParaDto(solicitacao);
        }

        public void Adicionar(Guid id, CriarSolicitacaoTransferenciaDto dto)
        {
            Validar.ValidarJustificativa(dto.Justificativa);

            Usuario usuario = _usuarioRepository.ObterPorId(id);

            if (usuario == null)
                throw new DomainException("Usuario não encontrado.");

            Patrimonio patrimonio = _repository.BuscarPatrimonioPorId(dto.PatrimonioID);

            if (patrimonio == null)
                throw new DomainException("Patrimonio não encontrado.");

            if (_repository.LocalizacaoExiste(dto.LocalizacaoID))
                throw new DomainException("Localizacao de destino não existe.");

            if (patrimonio.LocalizacaoID == dto.LocalizacaoID)
                throw new DomainException("O patrimônio já esta localizado nessa localização.");

            if (_repository.ExisteSolicitacaoPendente(dto.PatrimonioID))
                throw new DomainException("Já existe uma solicitação pendente para esse patrimônio.");

            if (usuario.TipoUsuario.NomeTipo == "Responsável")
            {
                bool usuarioReponsavel = _repository.UsuarioResponsavelDaLocalizacao(id, patrimonio.LocalizacaoID);

                if (!usuarioReponsavel)
                    throw new DomainException("O responsável só pode soliciatar tranferência de patrimônio do ambiente ao qual está vinculado.");
            }
            StatusTransferencia statusPendente = _repository.BuscarStatusTransferenciaPorNome("Pendente de aprovação");

            if (statusPendente == null)
                throw new DomainException("Status de transferência pendente não encontrado.");

            SolicitacaoTransferencia solicitacao = new SolicitacaoTransferencia
            {
                StatusTransferenciaID = statusPendente.StatusTransferenciaID,
                DataCriacaoSolicitante = DateTime.Now,
                Justificativa = dto.Justificativa,
                UsuarioIDSolicitacao = id,
                UsuarioIDAprovacao = null,
                PatrimonioID = dto.PatrimonioID,
                LocalizacaoID = dto.LocalizacaoID,
            };

            _repository.Adicionar(solicitacao);
        }

        public void Responder(Guid transferenciaId, Guid usuarioId, ResponderSolicitacaoTransferenciaDto dto)
        {
            Usuario usuario = _usuarioRepository.ObterPorId(transferenciaId);
            if (usuario == null)
                return;

            SolicitacaoTransferencia solicitacao = _repository.ObterPorId(transferenciaId);

            if (solicitacao == null)
                throw new DomainException("Solicitacao de transfernca não encontrada.");

            Patrimonio patrimonio = _repository.BuscarPatrimonioPorId(solicitacao.PatrimonioID);

            if (patrimonio == null)
                throw new DomainException("Patrimonio não encontrada.");

            StatusTransferencia statusPendente = _repository.BuscarStatusTransferenciaPorNome("Pendente de aprovação.");

            if (statusPendente == null)
                throw new DomainException("Status pendente não encontrado.");

            if (solicitacao.StatusTransferenciaID == statusPendente.StatusTransferenciaID)
                throw new DomainException("Essa solicitação ja foi respondida.");

            if (usuario.TipoUsuario.NomeTipo == "Responsável")
            {
                if (!_repository.UsuarioResponsavelDaLocalizacao(usuarioId, patrimonio.LocalizacaoID)) ;
                throw new DomainException("Somente o usuario responsável do ambiente de origem pode aprovar ou rejeitar essa solicitação.");
            }

            StatusTransferencia statusResposta;

            if (dto.Aprovado)
                statusResposta = _repository.BuscarStatusTransferenciaPorNome("Aprovado");

            else
                statusResposta = _repository.BuscarStatusTransferenciaPorNome("Recusado");

            if (statusResposta == null)
                throw new DomainException("Status de resposta de transferência não encontrado.");

            solicitacao.StatusTransferenciaID = statusResposta.StatusTransferenciaID;
            solicitacao.UsuarioIDAprovacao = usuarioId;
            solicitacao.DataResposta = DateTime.Now;

            _repository.Atualizar(solicitacao);

            if (dto.Aprovado)
            {
                StatusPatrimonio statusTransferido = _repository.BuscarStatusPatrimonioPorNome("Transferido");

                if (statusTransferido == null)
                    throw new DomainException("Status de patrimônio 'Transferido' não encontrado.");

                TipoAlteracao tipoAlteracao = _repository.BuscarTipoAlteracaoPorNome("Transferência");

                if (tipoAlteracao == null)
                    throw new DomainException("Tipo alteração 'Transferencia' não encontrado.");

                patrimonio.LocalizacaoID = solicitacao.LocalizacaoID;
                patrimonio.StatusPatrimonioID = statusTransferido.StatusPatrimonioID;

                _repository.AtualizarPatrimonio(patrimonio);

                LogPatrimonio log = new LogPatrimonio
                {
                    DataTransferencia = DateTime.Now,
                    PatrimonioID = patrimonio.PatrimonioID,
                    TipoAlteracaoID = tipoAlteracao.TipoAlteracaoID,
                    StatusPatrimonioID = statusTransferido.StatusPatrimonioID,
                    UsuarioID = usuarioId,
                    LocalizacaoID = patrimonio.LocalizacaoID,
                };

                _repository.AdicionarLog(log);
            }
        }
    }
}
