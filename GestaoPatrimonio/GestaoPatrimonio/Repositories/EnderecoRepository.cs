using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public EnderecoRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Endereco> Listar()
        {
            return _context.Endereco.ToList();
        }

        public Endereco ObterPorId(Guid id)
        {
            return _context.Endereco.Find(id);
        }

        public void Adicionar(Endereco endereco)
        {
            _context.Endereco.Add(endereco);
            _context.SaveChanges();
        }

        public void Atualizar(Endereco endereco)
        {
            Endereco nvEndereco = new Endereco
            {
                EnderecoID = endereco.EnderecoID,
                BairroID = endereco.BairroID,
                CEP = endereco.CEP,
                Logradouro = endereco.Logradouro,
                Complemento = endereco.Complemento,
                Numero = endereco.Numero,
            };

            _context.SaveChanges();
        }

        public bool BairroExiste(Guid id)
        {
            return _context.Bairro.Any(b => b.BairroID == id);
        }

        public List<Endereco> BuscarPorLougadouroENumero(string lougadouro, int? numero, Guid bairroId)
        {
            if(numero.HasValue)
            {
                return _context.Endereco.Where(e => e.BairroID == bairroId && e.Numero == numero && e.Logradouro == lougadouro).ToList();
            }

            return _context.Endereco.Where(e => e.BairroID == bairroId && e.Logradouro == lougadouro).ToList();
        }
    }
}
