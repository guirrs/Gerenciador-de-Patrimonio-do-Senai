using CsvHelper.Configuration;
using GestaoPatrimonios.DTOs.PatrimonioDto;

namespace GestaoPatrimonios.Applications.Mapeamentos
{
    // ClassMap -> é tipo um "tradutor de colunas", define como ler o csv
    public class ImportarPatrimonioCsvMap : ClassMap<ImportarPatrimonioCsvDto>
    {
        // definindo os mapeamentos
        public ImportarPatrimonioCsvMap()
        {
            // map -> escolhe a propriedade da DTO
            // Name -> diz qual o nome da coluna do CSV para essa propriedade
            Map(m => m.NumeroPatrimonio).Name("Nº invent.");
            Map(m => m.Denominacao).Name("Denominação do imobilizado");
            Map(m => m.DataIncorporacao).Name("Dt.incorp.");
            Map(m => m.ValorAquisicao).Name("ValAquis.");
        }
    }
}
