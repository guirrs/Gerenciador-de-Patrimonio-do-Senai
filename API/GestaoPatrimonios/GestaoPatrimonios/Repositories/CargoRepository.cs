using GestaoPatrimonios.Contexts;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.Interfaces;

namespace GestaoPatrimonios.Repositories
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
            return _context.Cargo
                .OrderBy(cargo => cargo.NomeCargo)
                .ToList();
        }

        public Cargo BuscarPorId(Guid cargoId)
        {
            return _context.Cargo.Find(cargoId);
        }

        public Cargo BuscarPorNome(string nomeCargo)
        {
            return _context.Cargo.FirstOrDefault(cargo => cargo.NomeCargo.ToLower() == nomeCargo.ToLower());
        }

        public void Adicionar(Cargo cargo)
        {
            _context.Cargo.Add(cargo);
            _context.SaveChanges();
        }

        public void Atualizar(Cargo cargo)
        {
            if (cargo == null)
            {
                return;
            }

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
