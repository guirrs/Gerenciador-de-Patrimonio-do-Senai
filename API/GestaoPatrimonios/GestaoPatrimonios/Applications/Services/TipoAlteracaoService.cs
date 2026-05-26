using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;
using GestaoPatrimonios_v1.DTOs.TipoAlteracaoDto;

namespace GestaoPatrimonios.Applications.Services
{
    public class TipoAlteracaoService
    {
        private readonly ITipoAlteracaoRepository _repository;

        public TipoAlteracaoService(ITipoAlteracaoRepository repository)
        {
            _repository = repository;
        }

        public List<ListarTipoAlteracaoDto> Listar()
        {
            List<TipoAlteracao> tiposAlteracao = _repository.Listar();

            List<ListarTipoAlteracaoDto> tiposAlteracaoDto = tiposAlteracao.Select(tipo => new ListarTipoAlteracaoDto
            {
                TipoAlteracaoID = tipo.TipoAlteracaoID,
                NomeTipo = tipo.NomeTipo
            }).ToList();

            return tiposAlteracaoDto;
        }

        public ListarTipoAlteracaoDto BuscarPorId(Guid tipoAlteracaoId)
        {
            TipoAlteracao tipoAlteracao = _repository.BuscarPorId(tipoAlteracaoId);

            if (tipoAlteracao == null)
            {
                throw new DomainException("Tipo de alteração não encontrado.");
            }

            ListarTipoAlteracaoDto tipoAlteracaoDto = new ListarTipoAlteracaoDto
            {
                TipoAlteracaoID = tipoAlteracao.TipoAlteracaoID,
                NomeTipo = tipoAlteracao.NomeTipo
            };

            return tipoAlteracaoDto;
        }

        public void Adicionar(CriarTipoAlteracaoDto dto)
        {
            Validar.ValidarNome(dto.NomeTipo);

            TipoAlteracao tipoExistente = _repository.BuscarPorNome(dto.NomeTipo);

            if (tipoExistente != null)
            {
                throw new DomainException("Já existe um tipo de alteração cadastrado com esse nome.");
            }

            TipoAlteracao tipoAlteracao = new TipoAlteracao
            {
                NomeTipo = dto.NomeTipo
            };

            _repository.Adicionar(tipoAlteracao);
        }

        public void Atualizar(Guid tipoAlteracaoId, CriarTipoAlteracaoDto dto)
        {
            Validar.ValidarNome(dto.NomeTipo);

            TipoAlteracao tipoBanco = _repository.BuscarPorId(tipoAlteracaoId);

            if (tipoBanco == null)
            {
                throw new DomainException("Tipo de alteração não encontrado.");
            }

            TipoAlteracao tipoExistente = _repository.BuscarPorNome(dto.NomeTipo);

            if (tipoExistente != null)
            {
                throw new DomainException("Já existe um tipo de alteração cadastrado com esse nome.");
            }

            tipoBanco.NomeTipo = dto.NomeTipo;

            _repository.Atualizar(tipoBanco);
        }
    }
}
