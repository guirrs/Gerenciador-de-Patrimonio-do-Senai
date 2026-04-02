using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Repositories
{
    public class StatusTransferenciaRepository : IStatusTransferenciaRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public StatusTransferenciaRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<StatusTransferencia> Listar()
        {
            return _context.StatusTransferencia.AsNoTracking().OrderBy(s => s.NomeStatus).ToList();
        }
        public StatusTransferencia ObterPorId(Guid id)
        {
            return _context.StatusTransferencia.Find(id);
        }

        public StatusTransferencia ObterPorNome(string nome)
        {
            return _context.StatusTransferencia.AsNoTracking().FirstOrDefault(s => s.NomeStatus == nome);
        }

        public void Adicionar(StatusTransferencia statusTransferencia)
        {
            _context.StatusTransferencia.Add(statusTransferencia);
            _context.SaveChanges();
        }

        public void Atualizar(StatusTransferencia statusTransferencia)
        {
            StatusTransferencia status = _context.StatusTransferencia.Find(statusTransferencia.StatusTransferenciaID);

            status.NomeStatus = statusTransferencia.NomeStatus;
            _context.SaveChanges();
        }
    }
}
