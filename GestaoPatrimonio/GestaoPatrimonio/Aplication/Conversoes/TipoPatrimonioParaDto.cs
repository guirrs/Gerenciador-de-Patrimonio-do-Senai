using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.TipoPatromonio;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class TipoPatrimonioParaDto
    {
        public static ListarTipoPatrimonioDto TipoPatrimonioListarParaDto(TipoPatrimonio tipo)
        {
            return new ListarTipoPatrimonioDto
            {
                TipoPatrimonioID = tipo.TipoPatrimonioID,
                NomeTipo = tipo.NomeTipo
            };
        }

        public static TipoPatrimonio DtoParaDomain(CriarTipoPatrimonioDto dto, Guid? id)
        {
            return new TipoPatrimonio
            {
                TipoPatrimonioID = id ?? Guid.Empty,
                NomeTipo = dto.NomeTipo
            };
        }
    }
}
