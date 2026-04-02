using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.TipoAlteracao;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class TipoAlteracaoService
    {
        private readonly ITipoAlteracaoRepository _repository;

        public TipoAlteracaoService(ITipoAlteracaoRepository repository)
        {
            _repository = repository;
        }

        public List<ListarTipoAlteracao> Listar()
        {
            return _repository.Listar().Select(t => TipoAlteracaoParaDto.TipoAlteracaoListarParaDto(t)).ToList();
        }

        public ListarTipoAlteracao ObterPorId(Guid id)
        {
            TipoAlteracao tipo = _repository.ObterPorId(id);

            if (tipo == null)
                throw new DomainException("ID não encontrado.");

            return TipoAlteracaoParaDto.TipoAlteracaoListarParaDto(tipo);
        }

        public void Adicionar(CriarTipoAlteracao dto)
        {
            if (_repository.ObterPorNome(dto.NomeTipo) == null)
                throw new CannotUnloadAppDomainException("Nome ja cadastrado.");

            _repository.Adicionar(TipoAlteracaoParaDto.DtoParaDomain(dto, null));
        }

        public void Atualizar(CriarTipoAlteracao dto, Guid id)
        {
            if (_repository.ObterPorNome(dto.NomeTipo) == null)
                throw new CannotUnloadAppDomainException("Nome ja cadastrado.");

            if (_repository.ObterPorId(id) == null)
                throw new DomainException("ID não encontrado.");

            _repository.Atualizar(TipoAlteracaoParaDto.DtoParaDomain(dto, id));
        }
    }
}
