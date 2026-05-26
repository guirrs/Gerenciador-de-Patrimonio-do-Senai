using GestaoPatrimonios.Contexts;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.Interfaces;

namespace GestaoPatrimonios.Repositories
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
            return _context.Endereco
                .OrderBy(endereco => endereco.Logradouro)
                .ToList();
        }

        public Endereco BuscarPorId(Guid enderecoId)
        {
            return _context.Endereco.Find(enderecoId);
        }

        public Endereco BuscarPorLogradouroENumero(
            string logradouro,
            int? numero,
            Guid bairroId,
            Guid? enderecoId = null
        )
        {
            var consulta = _context.Endereco.AsQueryable();

            if (enderecoId.HasValue)
            {
                consulta = consulta.Where(endereco => endereco.EnderecoID != enderecoId.Value);
            }

            return consulta.FirstOrDefault(endereco =>
                endereco.Logradouro.ToLower() == logradouro.ToLower() &&
                endereco.Numero == numero &&
                endereco.BairroID == bairroId
            );
        }

        public bool BairroExiste(Guid bairroId)
        {
            return _context.Bairro.Any(bairro => bairro.BairroID == bairroId);
        }

        public void Adicionar(Endereco endereco)
        {
            _context.Endereco.Add(endereco);
            _context.SaveChanges();
        }

        public void Atualizar(Endereco endereco)
        {
            Endereco enderecoBanco = _context.Endereco.Find(endereco.EnderecoID);

            if (enderecoBanco == null)
            {
                return;
            }

            enderecoBanco.Logradouro = endereco.Logradouro;
            enderecoBanco.Numero = endereco.Numero;
            enderecoBanco.Complemento = endereco.Complemento;
            enderecoBanco.CEP = endereco.CEP;
            enderecoBanco.BairroID = endereco.BairroID;

            _context.SaveChanges();
        }
    }
}