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
            return _context.Patrimonio.AsNoTracking().OrderBy(p => p.Denominacao).ToList();
        }

        public Patrimonio ObterPorId(Guid id)
        {
            return _context.Patrimonio.Find(id);
        }

        public Patrimonio ObterPorDenominacao(string nome)
        {
            return _context.Patrimonio.AsNoTracking().FirstOrDefault(p => p.Denominacao == nome);
        }

        public bool BuscarPorNumeroPatrimonio(string Numero)
        {
            return _context.Patrimonio.Any(p => p.NumeroPatrimonio == Numero);
        }

        public bool LocalizacaoExiste(Guid localizacaoId)
        {
            return _context.Localizacao.Any(l => l.LocalizacaoID == localizacaoId);
        }

        public bool StatusPatrimonioExiste(Guid statusId)
        {
            return _context.StatusPatrimonio.Any(status => status.StatusPatrimonioID == statusId);
        }

        public Localizacao BuscarLocalizacaoPorNome(string nome)
        {
            return _context.Localizacao.FirstOrDefault(localizacao => localizacao.NomeLocal.ToLower() == nome.ToLower());
        }

        public StatusPatrimonio BuscarStatusPatrimonioPorNome(string nome)
        {
            return _context.StatusPatrimonio.FirstOrDefault(status => status.NomeStatus.ToLower() == nome.ToLower());
        }

        public TipoAlteracao BuscarTipoAlteracaoPorNome(string nome)
        {
            return _context.TipoAlteracao.FirstOrDefault(tipo => tipo.NomeTipo.ToLower() == nome.ToLower());
        }

        public void Adicionar(Patrimonio patrimonio)
        {
            _context.Patrimonio.Add(patrimonio);
            _context.SaveChanges();
        }

        public void AtualizarStatus(Patrimonio patrimonio)
        {
            if (patrimonio == null)
            {
                return;
            }

            Patrimonio patrimonioBanco = _context.Patrimonio.Find(patrimonio.PatrimonioID);

            if (patrimonioBanco == null)
            {
                return;
            }

            patrimonioBanco.StatusPatrimonioID = patrimonio.StatusPatrimonioID;

            _context.SaveChanges();
        }

        public void AdicionarLog(LogPatrimonio logPatrimonio)
        {
            _context.LogPatrimonio.Add(logPatrimonio);
            _context.SaveChanges();
        }
    }
}
