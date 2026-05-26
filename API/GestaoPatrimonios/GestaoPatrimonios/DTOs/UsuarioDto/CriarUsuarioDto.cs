namespace GestaoPatrimonios.DTOs.UsuarioDto
{
    public class CriarUsuarioDto
    {
        public string NIF { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? RG { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string CarteiraTrabalho { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public Guid CargoID { get; set; }
        public Guid TipoUsuarioID { get; set; }

        public string Logradouro { get; set; } = string.Empty;
        public int? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? CEP { get; set; }

        public string NomeBairro { get; set; } = string.Empty;
        public string NomeCidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}