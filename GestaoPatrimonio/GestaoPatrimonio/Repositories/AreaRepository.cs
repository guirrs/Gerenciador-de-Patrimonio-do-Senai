using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

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
            return _context.Area.OrderBy(area => area.NomeArea).AsNoTracking().ToList();
        }

        public Area BuscarPorId(Guid id)
        {
            return _context.Area.Find(id);
        }

        public Area BuscarPorNome(string nome)
        {
            return _context.Area.AsNoTracking().FirstOrDefault(n => n.NomeArea.ToLower() == nome.ToLower());
        }

        public void Adicionar(Area area)
        {
            _context.Area.Add(area);
            _context.SaveChanges(); 
        }

        public void Atualizar(Area area)
        {
            if(area == null)
            {
                return;
            }

            Area areaBanco = _context.Area.Find(area.AreaID);

            if(areaBanco == null)
            {
                return;
            }

            areaBanco.NomeArea = area.NomeArea;
            _context.SaveChanges();
        }
    }
}
