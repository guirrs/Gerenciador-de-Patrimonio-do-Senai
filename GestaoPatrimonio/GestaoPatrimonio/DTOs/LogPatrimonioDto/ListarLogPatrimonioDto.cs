namespace GestaoPatrimonio.DTOs.LogPatrimonioDto
{
    public class ListarLogPatrimonioDto
    {
        public Guid LogPatrimonioId {  get; set; }
        public DateTime DataTransferencia { get; set; }
        public Guid PatrimonioId {  get; set; }
        public string DenomoinacaoPatrimonio { get; set; } = string.Empty;
        public string TipoAlteracao { get; set; } = string.Empty;
        public string StautusPatrimonio { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Local { get; set; } = string.Empty;
    }
}
