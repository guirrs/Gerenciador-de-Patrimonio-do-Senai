using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Repositories
{
    public class UsuarioRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public UsuarioRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Usuario> Listar()
        {
            return _context.Usuario.ToList();
        }
    }
}
