using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Repositories
{
    public class TipoAlteracaoRepository : ITipoAlteracaoRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public TipoAlteracaoRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<TipoAlteracao> Listar()
        {
            return _context.TipoAlteracao.AsNoTracking().OrderBy(t => t.NomeTipo).ToList();
        }

        public TipoAlteracao ObterPorId(Guid id)
        {
            return _context.TipoAlteracao.Find(id);
        }

        public TipoAlteracao ObterPorNome(string nome)
        {
            return _context.TipoAlteracao.AsNoTracking().FirstOrDefault(t => t.NomeTipo == nome);
        }

        public void Adicionar(TipoAlteracao alteracao)
        {
            _context.TipoAlteracao.Add(alteracao);
            _context.SaveChanges();
        }

        public void Atualizar(TipoAlteracao alteracao)
        {
            if( alteracao == null ) 
                return;

            if (alteracao.TipoAlteracaoID == null)
                return;

            TipoAlteracao alteracaoBanco = _context.TipoAlteracao.Find(alteracao.TipoAlteracaoID);

            alteracaoBanco.NomeTipo = alteracao.NomeTipo;

            _context.SaveChanges();
        }
    }
}
