using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.TipoUsuarioDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class TipoUsuarioParaDto
    {
        public static ListarTipoUsuarioDto TipoUsuarioListarParaDto(TipoUsuario tipo)
        {
            return new ListarTipoUsuarioDto
            {
                TipoUsuarioID = tipo.TipoUsuarioID,
                NomeTipo = tipo.NomeTipo
            };
        }

        public static CriarTipoUsuariodto TipoUsuarioCriarParaDto(TipoUsuario tipo)
        {
            return new CriarTipoUsuariodto
            {
                NomeTipo = tipo.NomeTipo
            };
        }
    }
}
