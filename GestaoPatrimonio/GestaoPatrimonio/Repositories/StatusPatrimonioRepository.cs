using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Repositories
{
    public class StatusPatrimonioRepository : IStatusPatrimonioRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public StatusPatrimonioRepository(GestaoPatrimoniosContext context)
        {
            _context = context; 
        }

        public List<StatusPatrimonio> Listar()
        {
            return _context.StatusPatrimonio.AsNoTracking().OrderBy(s => s.NomeStatus).ToList();  
        }

        public StatusPatrimonio ObterPorId(Guid id)
        {
            return _context.StatusPatrimonio.Find(id);
        }

        public StatusPatrimonio ObterPorNome(string nome)
        {
            return _context.StatusPatrimonio.AsNoTracking().FirstOrDefault(s => s.NomeStatus == nome);
        }

        public void Adicionar(StatusPatrimonio status)
        {
            _context.StatusPatrimonio.Add(status);
            _context.SaveChanges();
        }

        public void Atualizar(StatusPatrimonio status)
        {
            StatusPatrimonio statusBanco = _context.StatusPatrimonio.Find(status.StatusPatrimonioID);

            statusBanco.NomeStatus = status.NomeStatus;
            
            _context.SaveChanges();
        }
    }
}
