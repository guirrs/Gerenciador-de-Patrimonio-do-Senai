namespace GestaoPatrimonio.DTOs.Localizacao
{
    public class CriarLocalizacaoDto
    {
        public string NomeLocal {  get; set; } = string.Empty;
        public int LocalSAP { get; set; }
        public string? DescricacaoSAP {  get; set; }
        public Guid AreaID { get; set; }
    }
}
