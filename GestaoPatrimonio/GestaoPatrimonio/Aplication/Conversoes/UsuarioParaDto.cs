using GestaoPatrimonio.Aplication.Autenticacao;
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
                CargoID = usuario.CargoID,
                NomeCargo = usuario.Cargo.NomeCargo,
                TipoUsuarioID = usuario.TipoUsuarioID,
                NomeTipoUsuario = usuario.TipoUsuario.NomeTipo
            };
        }

        public static Usuario DtoParaDomain(CriarUsuarioDto dto, Guid? id)
        {
            return new Usuario
            {
                UsuarioID = id ?? Guid.Empty,
                CarteiraTrabalho = dto.CarteiraTrabalho,
                CPF = dto.CPF,
                NIF = dto.NIF,
                Email = dto.Email,
                Nome = dto.Nome,
                RG = dto.RG,
                TipoUsuarioID = dto.TipoUsuarioID,
                EnderecoID = dto.EnderecoID,
                PrimeiroAcesso = true,
                Senha = CriptografiaUsuario.CriptografiaSenha(dto.NIF),
                CargoID = dto.CargoID,
            };
        }
    }
}
