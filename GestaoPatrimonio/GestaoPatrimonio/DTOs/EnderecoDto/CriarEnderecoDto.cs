namespace GestaoPatrimonio.DTOs.EnderecoDto
{
    public class CriarEnderecoDto
    {
        public string Logradouro { get; set; } = null!;
        public int? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? CEP { get; set; }
        public Guid BairroID { get; set; }
    }
}
