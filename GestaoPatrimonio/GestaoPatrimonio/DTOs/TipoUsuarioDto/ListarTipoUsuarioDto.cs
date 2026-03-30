using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.DTOs.TipoUsuarioDto
{
    public class ListarTipoUsuarioDto
    {
        public Guid TipoUsuarioID { get; set; }
        public string NomeTipo { get; set; } = string.Empty;
    }
}
