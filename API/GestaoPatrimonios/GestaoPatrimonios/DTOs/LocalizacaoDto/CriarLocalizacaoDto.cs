namespace GestaoPatrimonios.DTOs.LocalizacaoDto
{
    public class CriarLocalizacaoDto
    {
        public string NomeLocal { get; set; } = string.Empty;
        public int LocalSAP { get; set; }
        public string DescricaoSAP { get; set; }
        public Guid AreaID { get; set; }
        public Guid UsuarioID { get; set; }

        //// se precisar que mais de um usuário seja responsável pelo local
        ///deixando aqui, caso a regra mude
        //public List<Guid> UsuariosIDs { get; set; } = new();
    }
}
