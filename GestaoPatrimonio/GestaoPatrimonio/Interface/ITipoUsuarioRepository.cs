using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ITipoUsuarioRepository
    {
        List<TipoUsuario> Listar();
        TipoUsuario ObterPorId(Guid id);
        TipoUsuario ObterPorNome(string nome);
        void Adicionar(TipoUsuario tipoUsuario);
        void Atualizar(TipoUsuario tipoUsuario);

    }
}
