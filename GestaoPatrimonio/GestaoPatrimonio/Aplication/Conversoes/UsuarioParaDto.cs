using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.UsuarioDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class UsuarioParaDto
    {
        public static ListarUsuarioDto ConverterParaDto(Usuario usuario)
        {
            return new ListarUsuarioDto
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
            };
        }
    }
}
