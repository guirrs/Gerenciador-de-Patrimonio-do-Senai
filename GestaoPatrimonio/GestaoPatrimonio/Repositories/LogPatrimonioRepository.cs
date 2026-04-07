using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Repositories
{
    public class LogPatrimonioRepository : ILogPatrimonioRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public LogPatrimonioRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<LogPatrimonio> Listar()
        {
            return _context.LogPatrimonio
                .Include(l => l.Usuario)
                .Include(l => l.Patrimonio)
                .Include(l => l.Localizacao)
                .Include(l => l.StatusPatrimonio)
                .Include(l => l.TipoAlteracao)
                .OrderByDescending(l => l.DataTransferencia)
                .ToList();
        }

        public List<LogPatrimonio> BuscarPorPatrimonio(Guid id)
        {
            return _context.LogPatrimonio
                .Include(l => l.Usuario)
                .Include(l => l.Patrimonio)
                .Include(l => l.Localizacao)
                .Include(l => l.StatusPatrimonio)
                .Include(l => l.TipoAlteracao)
                .OrderByDescending(l => l.DataTransferencia)
                .Where(l => l.PatrimonioID == id)
                .ToList();
        }
    }
}
