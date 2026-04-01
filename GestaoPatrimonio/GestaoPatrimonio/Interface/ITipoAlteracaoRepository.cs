using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ITipoAlteracaoRepository
    {
        List<TipoAlteracao> Listar();
        TipoAlteracao ObterPorId(Guid id);
        TipoAlteracao ObterPorNome(string nome);
        void Adicionar(TipoAlteracao tipo);
        void Atualizar(TipoAlteracao tipo);
    }
}
