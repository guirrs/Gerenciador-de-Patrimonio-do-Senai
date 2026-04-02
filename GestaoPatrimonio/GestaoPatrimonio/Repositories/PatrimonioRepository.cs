using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Repositories
{
    public class PatrimonioRepository : IPatrimonioRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public PatrimonioRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Patrimonio> Listar()
        {
            return _context.Patrimonio.AsNoTracking().ToList();
        }

        public Patrimonio ObterPorId(Guid id)
        {
            return _context.Patrimonio.Find(id);
        }

        public Patrimonio ObterPorDenominacao(string nome)
        {
            return _context.Patrimonio.AsNoTracking().FirstOrDefault(p => p.Denominacao == nome);
        }

        public void Adicionar(Patrimonio patrimonio)
        {
            _context.Patrimonio.Add(patrimonio);
            _context.SaveChanges();
        }

        public void Atualizar(Patrimonio patrimonio)
        {
            Patrimonio patrimonioBanco = _context.Patrimonio.Find(patrimonio.PatrimonioID);

            if (patrimonioBanco == null)
                return;

            patrimonioBanco.LocalizacaoID = patrimonio.PatrimonioID;
            patrimonioBanco.StatusPatrimonioID = patrimonio.StatusPatrimonioID;
            patrimonioBanco.TipoPatrimonioID = patrimonio.PatrimonioID;
            patrimonioBanco.NumeroPatrimonio = patrimonio.NumeroPatrimonio;
            patrimonioBanco.Denominacao = patrimonio.Denominacao;

            _context.SaveChanges();
        }

        public void Remover(Guid id)
        {
            Patrimonio patrimonio = _context.Patrimonio.Find(id);

            _context.Patrimonio.Remove(patrimonio);
            _context.SaveChanges();
        }
    }
}
