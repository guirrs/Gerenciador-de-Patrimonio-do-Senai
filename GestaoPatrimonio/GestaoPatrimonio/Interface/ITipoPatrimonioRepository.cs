using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ITipoPatrimonioRepository
    {
        public List<TipoPatrimonio> Listar();
        public TipoPatrimonio ObterPorId(Guid id);
        public TipoPatrimonio ObterPorNome(string nome);
        public void Adicionar(TipoPatrimonio tipoPatrimonio);
        public void Atualizar(TipoPatrimonio tipoPatrimonio);
    }
}
