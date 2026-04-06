namespace GestaoPatrimonio.DTOs.UsuarioDto
{
    public class ListarUsuarioDto
    {
        public Guid UsuarioID { get; set; }
        public string NIF { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? RG { get; set; }
        public string CPF { get; set; } = string.Empty;
        public string CarteiraTrabalho { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool? Ativo { get; set; }
        public Guid EnderecoID { get; set; }
        public Guid CargoID { get; set; }
        public string NomeCargo {  get; set; }
        public Guid TipoUsuarioID { get; set; }
        public string NomeTipoUsuario { get; set; }
    }
}
