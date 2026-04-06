using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

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
            return _context.Endereco.AsNoTracking().OrderBy(e => e.Logradouro)
                .Include(e => e.Bairro)
                .Include(e => e.Bairro.Cidade)
                .ToList();
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
            if (endereco == null)
                return;

            if (endereco.EnderecoID == null)
                return;

            Endereco enderecoBanco = _context.Endereco.Find(endereco.EnderecoID);

            enderecoBanco.Logradouro = endereco.Logradouro;
            enderecoBanco.CEP = endereco.CEP;
            enderecoBanco.Numero = endereco.Numero;
            enderecoBanco.BairroID = endereco.BairroID; 
            enderecoBanco.Complemento = endereco.Complemento;
            
            _context.SaveChanges();
        }

        public bool BairroExiste(Guid id)
        {
            return _context.Bairro.Any(b => b.BairroID == id);
        }

        public Endereco BuscarPorLougadouroENumero(string lougadouro, int? numero, Guid bairroId, Guid? enderecoId = null)
        {
            var consulta = _context.Endereco.AsNoTracking().AsQueryable();

            if (enderecoId.HasValue)
                consulta = consulta.AsNoTracking().Where(e => e.EnderecoID == enderecoId);

            return consulta.AsNoTracking().FirstOrDefault(e =>
            e.Logradouro.ToLower() == lougadouro.ToLower()
            && e.Numero == numero
            && e.BairroID == bairroId);
        }
    }
}
