using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.CidadeDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class CidadeParaDto
    {
        public static ListarCidadeDto ConverterParaDto(Cidade cidade)
        {
            return new ListarCidadeDto
            {
                NomeCidade = cidade.NomeCidade,
                Estado = cidade.Estado,
            };
        }
    }
}
