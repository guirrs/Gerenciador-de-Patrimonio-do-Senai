using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ITipoUsuarioRepository
    {
        List<TipoUsuario> Listar();
        TipoUsuario ObterPorId(Guid id);
        void Adicionar(TipoUsuario tipoUsuario);
        void Atualizar(TipoUsuario tipoUsuario);

    }
}
