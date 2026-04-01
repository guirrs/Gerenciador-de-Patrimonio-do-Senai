using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.Localizacao;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class LocalizacaoParaDto
    {
        public static ListarLocalizacaoDto ConverterParaDto(Localizacao l)
        {
            return new ListarLocalizacaoDto
            {
                ID = l.LocalizacaoID,
                NomeLocal = l.NomeLocal,
                DescricaoSAP = l.DescricaoSAP,
                LocalSAP = l.LocalSAP,
                AreaID = l.AreaID,
            };
        }
        public static ListarLocalizacaoDto CriarDto(Localizacao l)
        {
            return new ListarLocalizacaoDto
            {
                ID = l.LocalizacaoID,
                NomeLocal = l.NomeLocal,
                DescricaoSAP = l.DescricaoSAP,
                LocalSAP = l.LocalSAP,
                AreaID = l.AreaID,
            };
        }
    }
}
