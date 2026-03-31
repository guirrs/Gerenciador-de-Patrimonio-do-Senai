using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.UsuarioDto;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();

            List<ListarUsuarioDto> usuariosDto = usuarios.Select(usuario => new ListarUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                NIF = usuario.NIF,
                CarteiraTrabalho = usuario.CarteiraTrabalho,
                CPF = usuario.CPF,
                Email = usuario.Email,
                Nome = usuario.Nome,
                RG = usuario.RG,
                EnderecoID = usuario.EnderecoID,
                Ativo = usuario.Ativo,
            });
        }
    }
}
