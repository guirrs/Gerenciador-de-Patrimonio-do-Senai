using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Repositories
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
            return _context.TipoUsuario.AsNoTracking().OrderBy(u => u.NomeTipo).ToList();
        }
        public TipoUsuario ObterPorId(Guid id)
        {
            return _context.TipoUsuario.Find(id);
        }
        public TipoUsuario ObterPorNome(string nome)
        {
            return _context.TipoUsuario.AsNoTracking().FirstOrDefault(t => t.NomeTipo == nome);
        }

        public void Adicionar(TipoUsuario tipoUsuario)
        {
            _context.TipoUsuario.Add(tipoUsuario);
            _context.SaveChanges();
        }

        public void Atualizar(TipoUsuario tipoUsuario)
        {
           TipoUsuario tipoBanco = _context.TipoUsuario.Find(tipoUsuario.TipoUsuarioID);

            tipoBanco.NomeTipo = tipoUsuario.NomeTipo;

            _context.SaveChanges();
        }
    }
}
