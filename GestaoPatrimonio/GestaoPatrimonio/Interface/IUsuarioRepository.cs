using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface IUsuarioRepository
    {
        List<Usuario> Listar();
    }
}
