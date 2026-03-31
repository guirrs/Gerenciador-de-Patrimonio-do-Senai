namespace GestaoPatrimonio.DTOs.StatusTransferencia
{
    public class ListarStatusTransferenciaDto
    {
        public Guid StatusPatrimonioID { get; set; }
        public string NomeStatus { get; set; } = null!;
    }
}
