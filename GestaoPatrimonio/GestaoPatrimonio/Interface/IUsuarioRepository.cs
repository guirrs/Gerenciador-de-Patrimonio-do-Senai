using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface IUsuarioRepository
    {
        List<Usuario> Listar();
        Usuario ObterPorId(Guid id);
        Usuario ObterPorNome(string nome);
        void Adicionar(Usuario usuario);
        void Atualizar(Usuario usuario);
        void Remover(Usuario usuario);
    }
}
