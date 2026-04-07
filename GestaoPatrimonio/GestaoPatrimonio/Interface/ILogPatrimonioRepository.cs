using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ILogPatrimonioRepository
    {
        List<LogPatrimonio> Listar();
        List<LogPatrimonio> BuscarPorPatrimonio(Guid id);
    }
}
