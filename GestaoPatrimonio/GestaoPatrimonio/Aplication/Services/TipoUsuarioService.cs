using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.TipoUsuarioDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class TipoUsuarioService
    {
        private readonly ITipoUsuarioRepository _repository;

        public TipoUsuarioService(ITipoUsuarioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarTipoUsuarioDto> Listar()
        {
            return _repository.Listar().Select(t => TipoUsuarioParaDto.TipoUsuarioListarParaDto(t)).ToList();
        }

        public ListarTipoUsuarioDto ObterPorId(Guid id)
        {
            ListarTipoUsuarioDto dto = TipoUsuarioParaDto.TipoUsuarioListarParaDto(_repository.ObterPorId(id));

            if(dto == null)
            {
                throw new DomainException("Tipo usuario nao encontrado");
            }

            return dto;
        }

        public void Adicionar(CriarTipoUsuariodto dto)
        {
            TipoUsuario nomeE = _repository.ObterPorNome(dto.NomeTipo);

            if (nomeE != null)
                throw new DomainException("Tipo ja cadastrado");

            _repository.Adicionar(TipoUsuarioParaDto.DtoParaDomain(dto, null));
        }

        public void Atualizar(CriarTipoUsuariodto dto, Guid id)
        {
            if (_repository.ObterPorId(id) == null)
                throw new DomainException("ID não encontrado.");
            _repository.Atualizar(TipoUsuarioParaDto.DtoParaDomain(dto, id));
        }
    }
}
