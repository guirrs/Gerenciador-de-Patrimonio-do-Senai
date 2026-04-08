namespace GestaoPatrimonio.DTOs.SolicitacaoTransferenciaDto
{
    public class ListarSolicitacaoTransferenciaDto
    {
        public Guid TransferenciaID { get; set; }
        public DateTime DataCriacaoSolicitante { get; set; }
        public DateTime? DataResposta { get; set; }
        public string Justificativa { get; set; } = null!;
        public Guid StatusTransferenciaID { get; set; }
        public Guid UsuarioIDSolicitacao { get; set; }
        public string NomeUsuarioSolicitacao { get; set; }
        public Guid? UsuarioIDAprovacao { get; set; }
        public string NomeUsuarioAprovacao { get; set; }
        public Guid PatrimonioID { get; set; }
        public string NomePatrimonio { get; set; }
        public Guid LocalizacaoID { get; set; }
    }
}
