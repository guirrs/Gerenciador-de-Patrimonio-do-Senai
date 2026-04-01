using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.DTOs.CargoDto
{
    public class ListarCargoDto
    {
        public Guid CargoID { get; set; }
        public string NomeCargo { get; set; } = string.Empty;
    }
}
