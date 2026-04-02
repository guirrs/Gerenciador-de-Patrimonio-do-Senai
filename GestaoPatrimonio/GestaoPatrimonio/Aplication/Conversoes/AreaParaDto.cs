using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.AreaDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class AreaParaDto
    {
        public static ListarAreaDto ConverterParaDto(Area area)
        {
            return new ListarAreaDto
            {
                AreaID = area.AreaID,
                NomeArea = area.NomeArea,
            };
        }

        public static Area DtoParaDomain(CriarAreaDto area, Guid? id)
        {
            return new Area
            {
                AreaID = id ?? Guid.Empty,
                NomeArea = area.NomeArea,
            };
}
    }
}
