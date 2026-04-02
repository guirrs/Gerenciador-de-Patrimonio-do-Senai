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

        public static StatusTransferencia DtoParaDomain(CriarStatusTransferenciaDto dto, Guid? id)
        {
            return new StatusTransferencia
            {
                StatusTransferenciaID = id ?? Guid.Empty,
                NomeStatus = dto.NomeStatus,
            };
        }
    }
}
