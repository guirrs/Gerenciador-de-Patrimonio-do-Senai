using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.StatusTransferencia;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class StatusTransferenciaParaDto
    {
        public static ListarStatusTransferenciaDto StatusListarParaDto(StatusTransferencia status)
        {
            return new ListarStatusTransferenciaDto
            {
                StatusPatrimonioID = status.StatusTransferenciaID,
                NomeStatus = status.NomeStatus,
            };
        }

        public static CriarStatusTransferenciaDto StatusCriarParaDto(StatusTransferencia status)
        {
            return new CriarStatusTransferenciaDto
            {
                NomeStatus = status.NomeStatus
            };
        }
    }
}
