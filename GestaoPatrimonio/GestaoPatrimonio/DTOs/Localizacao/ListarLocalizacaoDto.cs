namespace GestaoPatrimonio.DTOs.Localizacao
{
    public class ListarLocalizacaoDto
    {
        public Guid ID { get; set; }
        public string NomeLocal {  get; set; } = string.Empty;
        public int? LocalSAP { get; set; }
        public string DescricaoSAP { get; set; }
        public Guid AreaID { get; set; }
    }
}
