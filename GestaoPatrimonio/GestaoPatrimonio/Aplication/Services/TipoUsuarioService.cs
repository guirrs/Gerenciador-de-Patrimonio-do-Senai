using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.TipoUsuarioDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Repositories;

namespace GestaoPatrimonio.Aplication.Services
{
    public class TipoUsuarioService
    {
        private readonly TipoUsuarioRepository _repository;

        public TipoUsuarioService(TipoUsuarioRepository repository)
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
            TipoUsuario tipoBanco = new TipoUsuario
            {
                NomeTipo = dto.NomeTipo
            };

            _repository.Adicionar(tipoBanco);
        }

        public void Adicionar(CriarTipoUsuariodto dto, Guid id)
        {
            TipoUsuario tipo = _repository.ObterPorId(id);

            TipoUsuario tipoBanco = new TipoUsuario
            {
                NomeTipo = tipo.NomeTipo
            };

            _repository.Adicionar(tipoBanco);
        }
    }
}
