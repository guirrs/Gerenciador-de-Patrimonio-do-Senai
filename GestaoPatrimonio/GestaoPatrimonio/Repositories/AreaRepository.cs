using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public AreaRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }
        public List<Area> Listar()
        {
            return _context.Area.OrderBy(area => area.NomeArea).ToList();
        }
    }
}
