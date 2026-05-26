using GestaoPatrimonios.Contexts;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.Interfaces;

namespace GestaoPatrimonios.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public TipoUsuarioRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<TipoUsuario> Listar()
        {
            return _context.TipoUsuario
                .OrderBy(tipoUsuario => tipoUsuario.NomeTipo)
                .ToList();
        }

        public TipoUsuario BuscarPorId(Guid tipoUsuarioId)
        {
            return _context.TipoUsuario.Find(tipoUsuarioId);
        }

        public TipoUsuario BuscarPorNome(string nomeTipo)
        {
            return _context.TipoUsuario.FirstOrDefault(tipoUsuario => tipoUsuario.NomeTipo.ToLower() == nomeTipo.ToLower());
        }

        public void Adicionar(TipoUsuario tipoUsuario)
        {
            _context.TipoUsuario.Add(tipoUsuario);
            _context.SaveChanges();
        }

        public void Atualizar(TipoUsuario tipoUsuario)
        {
            if (tipoUsuario == null)
            {
                return;
            }

            TipoUsuario tipoUsuarioBanco = _context.TipoUsuario.Find(tipoUsuario.TipoUsuarioID);

            if (tipoUsuarioBanco == null)
            {
                return;
            }

            tipoUsuarioBanco.NomeTipo = tipoUsuario.NomeTipo;

            _context.SaveChanges();
        }
    }
}
