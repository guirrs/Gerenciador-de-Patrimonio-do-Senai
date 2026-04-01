using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.CargoDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
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
            return _repository.Listar().Select(c => CargoParaDto.ListarCargoParaDto(c)).ToList();
        }

        public ListarCargoDto ObterPorId(Guid id)
        {
            Cargo cargo = _repository.ObterPorId(id);

            if(cargo == null)
            {
                throw new DomainException("ID não encontrado");
            }

            return CargoParaDto.ListarCargoParaDto(cargo);
        }

        public void Adicionar(CriarCargoDto dto)
        {
            Cargo cargoNome = _repository.ObterPorNome(dto.NomeCargo);

            if (cargoNome != null)
                throw new DomainException("Cargo ja cadastrado.");

            _repository.Adicionar(new Cargo { NomeCargo = dto.NomeCargo});

        }

        public void Atualizar(CriarCargoDto dto, Guid id)
        {
            Cargo cargo = _repository.ObterPorId(id);
            Cargo cargoNome = _repository.ObterPorNome(dto.NomeCargo);

            if (cargo == null)
                throw new DomainException("Cargo não encontrado");

            if (cargoNome != null)
                throw new DomainException("Cargo ja cadastrado.");

            cargo.NomeCargo = dto.NomeCargo;

            _repository.Atualizar(cargo);
        }
    }
}
