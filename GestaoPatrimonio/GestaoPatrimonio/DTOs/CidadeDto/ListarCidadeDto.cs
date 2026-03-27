namespace GestaoPatrimonio.DTOs.CidadeDto
{
    public class ListarCidadeDto
    {
        public Guid CidadeID { get; set; }
        public string NomeCidade { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
