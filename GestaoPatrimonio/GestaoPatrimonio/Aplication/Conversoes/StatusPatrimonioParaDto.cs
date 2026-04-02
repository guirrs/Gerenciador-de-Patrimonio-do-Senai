using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.StatusPatrimonio;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class StatusPatrimonioParaDto
    {
        public static ListarStatusPatrimonioDto ListarStatusPatrimonioParaDto (StatusPatrimonio s)
        {
            return new ListarStatusPatrimonioDto
            {
                NomeStatus = s.NomeStatus,
                StatusPatrimonioID = s.StatusPatrimonioID,
            };
        }

        public static StatusPatrimonio DtoParaDomain(CriarStatusPatrimonioDto dto, Guid? id)
        {
            return new StatusPatrimonio
            {
                StatusPatrimonioID = id ?? Guid.Empty,
                NomeStatus = dto.NomeStatus,
            };
        }
    }
}
