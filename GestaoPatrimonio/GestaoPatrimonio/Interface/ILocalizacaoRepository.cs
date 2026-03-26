using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ILocalizacaoRepository
    {
        List<Localizacao> Listar();
        Localizacao ObterPorID(Guid id);
        bool NomeExiste(string nome);
        bool AreaExiste(Guid area);
        void Adicionar(Localizacao localizacao);
        void Atualizar(Localizacao localizacao);
    }
}
