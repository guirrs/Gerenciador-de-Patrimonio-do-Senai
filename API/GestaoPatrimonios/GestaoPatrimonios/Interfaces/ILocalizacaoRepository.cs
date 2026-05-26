using GestaoPatrimonios.Domains;

namespace GestaoPatrimonios.Interfaces
{
    public interface ILocalizacaoRepository
    {
        List<Localizacao> Listar();
        Localizacao BuscarPorId(Guid localizacaoId);
        void Adicionar(Localizacao localizacao);
        bool AreaExiste(Guid areaId);
        void Atualizar(Localizacao localizacao);
        Localizacao BuscarPorNome(string nomeLocal, Guid areaId);
        bool UsuarioExiste(Guid usuarioId);
        void VincularUsuario(Guid localizacaoId, Guid usuarioId);
    }
}
