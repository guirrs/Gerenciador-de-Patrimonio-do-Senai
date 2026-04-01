namespace GestaoPatrimonio.DTOs.PatrimonioDto
{
    public class CriarPatrimonioDto
    {
        public string Denominacao { get; set; } = null!;
        public string NumeroPatrimonio { get; set; } = null!;
        public decimal? Valor { get; set; }
        public string? Imagem { get; set; }
        public Guid LocalizacaoID { get; set; }
        public Guid TipoPatrimonioID { get; set; }
        public Guid StatusPatrimonioID { get; set; }
    }
}
