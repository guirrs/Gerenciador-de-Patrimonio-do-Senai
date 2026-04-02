using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.PatrimonioDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class PatrimonioParaDto
    {
        public static ListarPatrimonioDto ListarPatrimonioParaDto(Patrimonio dto)
        {
            return new ListarPatrimonioDto
            {
                Denominacao = dto.Denominacao,
                Imagem = dto.Imagem,
                LocalizacaoID = dto.LocalizacaoID,
                NumeroPatrimonio = dto.NumeroPatrimonio,
                PatrimonioID = dto.PatrimonioID,
                StatusPatrimonioID = dto.PatrimonioID,
                TipoPatrimonioID = dto.TipoPatrimonioID,
                Valor = dto.Valor,
            };
        }
        public static Patrimonio DtoParaDomain(CriarPatrimonioDto dto, Guid? id)
        {
            return new Patrimonio
            {
                PatrimonioID = id ?? Guid.Empty,
                Denominacao = dto.Denominacao,
                Imagem = dto.Imagem,
                LocalizacaoID = dto.LocalizacaoID,
                StatusPatrimonioID = dto.StatusPatrimonioID,
                NumeroPatrimonio = dto.NumeroPatrimonio,
                TipoPatrimonioID = dto.TipoPatrimonioID,
                Valor = dto.Valor,
            };
        }
    }
}
