using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.CargoDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class CargoParaDto
    {
        public static ListarCargoDto ListarCargoParaDto(Cargo c)
        {
            return new ListarCargoDto
            {
                CargoID = c.CargoID,
                NomeCargo = c.NomeCargo,
            };
        }

        public static Cargo DomainParaDto(CriarCargoDto dto, Guid? id)
        {
            return new Cargo
            {
                CargoID = id ?? Guid.Empty,
                NomeCargo = dto.NomeCargo
            };
        }
    }
}
