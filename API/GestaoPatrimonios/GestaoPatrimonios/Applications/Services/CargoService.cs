using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.DTOs.CargoDto;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;

namespace GestaoPatrimonios.Applications.Services
{
    public class CargoService
    {
        private readonly ICargoRepository _repository;

        public CargoService(ICargoRepository repository)
        {
            _repository = repository;
        }

        public List<ListarCargoDto> Listar()
        {
            List<Cargo> cargos = _repository.Listar();

            List<ListarCargoDto> cargosDto = cargos.Select(cargo => new ListarCargoDto
            {
                CargoID = cargo.CargoID,
                NomeCargo = cargo.NomeCargo
            }).ToList();

            return cargosDto;
        }

        public ListarCargoDto BuscarPorId(Guid cargoId)
        {
            Cargo cargo = _repository.BuscarPorId(cargoId);

            if (cargo == null)
            {
                throw new DomainException("Cargo não encontrado.");
            }

            ListarCargoDto cargoDto = new ListarCargoDto
            {
                CargoID = cargo.CargoID,
                NomeCargo = cargo.NomeCargo
            };

            return cargoDto;
        }

        public void Adicionar(CriarCargoDto dto)
        {
            Validar.ValidarNome(dto.NomeCargo);

            Cargo cargoExistente = _repository.BuscarPorNome(dto.NomeCargo);

            if (cargoExistente != null)
            {
                throw new DomainException("Já existe um cargo cadastrado com esse nome.");
            }

            Cargo cargo = new Cargo
            {
                NomeCargo = dto.NomeCargo
            };

            _repository.Adicionar(cargo);
        }

        public void Atualizar(Guid cargoId, CriarCargoDto dto)
        {
            Validar.ValidarNome(dto.NomeCargo);

            Cargo cargoBanco = _repository.BuscarPorId(cargoId);

            if (cargoBanco == null)
            {
                throw new DomainException("Cargo não encontrado.");
            }

            Cargo cargoExistente = _repository.BuscarPorNome(dto.NomeCargo);

            if (cargoExistente != null && cargoExistente.CargoID != cargoId)
            {
                throw new DomainException("Já existe um cargo cadastrado com esse nome.");
            }

            cargoBanco.NomeCargo = dto.NomeCargo;

            _repository.Atualizar(cargoBanco);
        }
    }
}
