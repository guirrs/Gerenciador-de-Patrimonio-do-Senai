using System.ComponentModel.DataAnnotations;

namespace GestaoPatrimonio.DTOs.AreaDto
{
    public class CriarAreaDto
    {
        [Required(ErrorMessage = "O nome da área é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Nome da area deve ter no maximo 50 caracteres.")]
        // string.Empty proibi totalmente o null e é melhor que o null!
        public string NomeArea { get; set; } = string.Empty;
    }
}
