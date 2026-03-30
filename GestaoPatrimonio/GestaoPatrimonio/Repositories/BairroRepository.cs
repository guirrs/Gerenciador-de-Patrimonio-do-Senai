using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Repositories
{
    public class BairroRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public BairroRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Bairro> Listar()
        {
            return _context.Bairro
                .ToList();
        }
        public Bairro BuscarPorId(Guid bairroID)
        {
            return _context.Bairro.Find(bairroID);
        }
        public void Adicionar(Bairro bairro)
        {
            _context.Bairro.Add(bairro);
            _context.SaveChanges();
        }
        public void Atualizar(Bairro bairro)
        {
            Bairro bairroBanco = _context.Bairro.Find(bairro.BairroID);

            bairroBanco.NomeBairro = bairro.NomeBairro;
            bairroBanco.CidadeID = bairro.CidadeID;
            bairroBanco.Endereco = bairro.Endereco;

            _context.SaveChanges();
        }
        public Bairro BuscarPorNome(string nome, Guid cidadeID)
        {
            return _context.Bairro.FirstOrDefault(e => e.NomeBairro == nome && e.CidadeID == cidadeID);
        }
        public bool CidadeExiste(Guid cidadeID)
        {
            return _context.Cidade.Any(c => c.CidadeID == cidadeID);
        }
    }
}
