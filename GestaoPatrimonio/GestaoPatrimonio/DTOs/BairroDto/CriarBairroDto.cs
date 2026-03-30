using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.DTOs.BairroDto
{
    public class CriarBairroDto
    {
        public string NomeBairro { get; set; } = null!;
        public Guid CidadeID { get; set; }
    }
}
