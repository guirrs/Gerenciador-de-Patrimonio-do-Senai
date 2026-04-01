using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Repositories
{
    public class CargoRepository : ICargoRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public CargoRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Cargo> Listar()
        {
            return _context.Cargo.OrderBy(c => c.NomeCargo).ToList();
        }

        public Cargo ObterPorId(Guid id)
        {
            return _context.Cargo.Find(id);
        }

        public Cargo ObterPorNome(string nome)
        {
            return _context.Cargo.FirstOrDefault(c => c.NomeCargo == nome);
        }

        public void Adicionar(Cargo cargo)
        {
            _context.Cargo.Add(cargo);
            _context.SaveChanges();
        }

        public void Atualizar(Cargo cargo)
        {
            Cargo cargoBanco = _context.Cargo.Find(cargo.CargoID);

            if (cargoBanco == null)
            {
                return;
            }

            cargoBanco.NomeCargo = cargo.NomeCargo;

            _context.SaveChanges();
        }
    }
}
