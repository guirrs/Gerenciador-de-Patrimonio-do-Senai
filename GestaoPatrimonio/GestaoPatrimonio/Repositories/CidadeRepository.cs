using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Repositories
{
    public class CidadeRepository : ICidadeRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public CidadeRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Cidade> Listar()
        {
            return _context.Cidade.OrderBy(c => c.NomeCidade).ToList();
        }

        public Cidade ObterPorId(Guid Id)
        {
            return _context.Cidade.Find(Id);
        }

        public Cidade ObterPorNome(string nome)
        {
            return _context.Cidade.FirstOrDefault(c => c.NomeCidade == nome);
        }

        public Cidade ObterPorNomeEEstado(string nomeCidade, string NomeEstado)
        {
            return _context.Cidade.FirstOrDefault(c => c.Estado == NomeEstado && c.NomeCidade == nomeCidade);
        }
    }
}
