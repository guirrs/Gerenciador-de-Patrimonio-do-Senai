using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.AreaDto;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Repositories
{
    public class TipoPatrimonioRepository : ITipoPatrimonioRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public TipoPatrimonioRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<TipoPatrimonio> Listar()
        {
            return _context.TipoPatrimonio.ToList();
        }

        public TipoPatrimonio ObterPorId(Guid id)
        {
            return _context.TipoPatrimonio.Find(id);
        }

        public TipoPatrimonio ObterPorNome(string nome)
        {
            return _context.TipoPatrimonio.FirstOrDefault(t => t.NomeTipo == nome);
        }

        public void Adicionar(TipoPatrimonio tipoPatrimonio)
        {
            _context.TipoPatrimonio.Add(tipoPatrimonio);
            _context.SaveChanges();
        }

        public void Atualizar(TipoPatrimonio tipoPatrimonio)
        {
            TipoPatrimonio tipo = _context.TipoPatrimonio.Find(tipoPatrimonio.TipoPatrimonioID);

            tipo.NomeTipo = tipoPatrimonio.NomeTipo;

            _context.SaveChanges();
        }
    }
}
