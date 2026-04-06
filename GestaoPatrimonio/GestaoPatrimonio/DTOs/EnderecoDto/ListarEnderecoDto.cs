namespace GestaoPatrimonio.DTOs.EnderecoDto
{
    public class ListarEnderecoDto
    {
        public Guid EnderecoID { get; set; }
        public string Logradouro { get; set; } = null!;
        public int? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? CEP { get; set; }
        public Guid BairroID { get; set; }
        public string NomeBairro { get; set; }
        public Guid CidadeId {  get; set; }
        public string NomeCidade {  get; set; }
    }
}
