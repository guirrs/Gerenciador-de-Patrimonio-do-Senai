namespace GestaoPatrimonio.DTOs.SolicitacaoTransferenciaDto
{
    public class CriarSolicitacaoTransferenciaDto
    {
        public DateTime DataCriacaoSolicitante { get; set; }
        public DateTime? DataResposta { get; set; }
        public string Justificativa { get; set; } = null!;
        public Guid StatusTransferenciaID { get; set; }
        public Guid UsuarioIDSolicitacao { get; set; }
        public Guid? UsuarioIDAprovacao { get; set; }
        public Guid PatrimonioID { get; set; }
        public Guid LocalizacaoID { get; set; }
    }
}
