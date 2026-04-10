using CsvHelper.Configuration;
using GestaoPatrimonio.DTOs.PatrimonioDto;

namespace GestaoPatrimonio.Aplication.Mapeamentos
{
    // ClassMap é como se fosse um tradutor de colunas
    public class ImportarPatrimonioCsvMap : ClassMap<ImportarPatrimonioCsvDto>
    {
        public ImportarPatrimonioCsvMap()
        {
            // map -> Escolhe a propriedade da DTO
            // Name -> Diz qual nome da coluna do CSV(Excel) para essa propriedade
            Map(m => m.NumeroPatrimonio).Name("N invent.");
            Map(m => m.Denominacao).Name("Denominação do imobilizado");
            Map(m => m.DataIncorporacao).Name("Dt.incorp.");
            Map(m => m.ValorAquisicao).Name("ValAquis.");
        }
    }
}
