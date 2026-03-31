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
        public static CriarTipoPatrimonioDto TipoPatrimonioCriarParaDto(TipoPatrimonio tipo)
        {
            return new CriarTipoPatrimonioDto
            {
                NomeTipo = tipo.NomeTipo
            };
        }
    }
}
