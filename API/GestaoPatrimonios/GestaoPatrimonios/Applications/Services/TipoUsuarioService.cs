using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;
using GestaoPatrimonios_v1.DTOs.TipoUsuarioDto;

namespace GestaoPatrimonios.Applications.Services
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
            List<TipoUsuario> tiposUsuario = _repository.Listar();

            List<ListarTipoUsuarioDto> tiposUsuarioDto = tiposUsuario.Select(tipoUsuario => new ListarTipoUsuarioDto
            {
                TipoUsuarioID = tipoUsuario.TipoUsuarioID,
                NomeTipo = tipoUsuario.NomeTipo
            }).ToList();

            return tiposUsuarioDto;
        }

        public ListarTipoUsuarioDto BuscarPorId(Guid tipoUsuarioId)
        {
            TipoUsuario tipoUsuario = _repository.BuscarPorId(tipoUsuarioId);

            if (tipoUsuario == null)
            {
                throw new DomainException("Tipo de usuário não encontrado.");
            }

            ListarTipoUsuarioDto tipoUsuarioDto = new ListarTipoUsuarioDto
            {
                TipoUsuarioID = tipoUsuario.TipoUsuarioID,
                NomeTipo = tipoUsuario.NomeTipo
            };

            return tipoUsuarioDto;
        }

        public void Adicionar(CriarTipoUsuarioDto dto)
        {
            Validar.ValidarNome(dto.NomeTipo);

            TipoUsuario tipoUsuarioExistente = _repository.BuscarPorNome(dto.NomeTipo);

            if (tipoUsuarioExistente != null)
            {
                throw new DomainException("Já existe um tipo de usuário cadastrado com esse nome.");
            }

            TipoUsuario tipoUsuario = new TipoUsuario
            {
                NomeTipo = dto.NomeTipo
            };

            _repository.Adicionar(tipoUsuario);
        }

        public void Atualizar(Guid tipoUsuarioId, CriarTipoUsuarioDto dto)
        {
            Validar.ValidarNome(dto.NomeTipo);

            TipoUsuario tipoUsuarioBanco = _repository.BuscarPorId(tipoUsuarioId);

            if (tipoUsuarioBanco == null)
            {
                throw new DomainException("Tipo de usuário não encontrado.");
            }

            TipoUsuario tipoUsuarioExistente = _repository.BuscarPorNome(dto.NomeTipo);

            if (tipoUsuarioExistente != null)
            {
                throw new DomainException("Já existe um tipo de usuário cadastrado com esse nome.");
            }

            tipoUsuarioBanco.NomeTipo = dto.NomeTipo;

            _repository.Atualizar(tipoUsuarioBanco);
        }
    }
}
