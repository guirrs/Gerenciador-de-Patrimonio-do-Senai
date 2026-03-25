using GestaoPatrimonio.Interface;
using GestaoPatrimonio.DTOs.AreaDto;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Aplication.Regras;

namespace GestaoPatrimonio.Aplication.Services
{
    public class AreaService
    {
        private readonly IAreaRepository _repository;
        public AreaService(IAreaRepository repository)
        {
            _repository = repository;
        }


        public List<ListarAreaDto> Listar()
        {
            return _repository.Listar().Select(area =>
            new ListarAreaDto
            {
                AreaID = area.AreaID,
                NomeArea = area.NomeArea,
            }).ToList();
        }

        public ListarAreaDto BuscarPorId(Guid id)
        {
            Area area = _repository.BuscarPorId(id);

            if(area == null)
            {
                throw new DomainException("Area não encontrada");
            }

            ListarAreaDto areaDto = new ListarAreaDto {
                AreaID  = area.AreaID,
                NomeArea = area.NomeArea,
            };

            return areaDto;
        }

        public void Adicionar(CriarAreaDto areaDto)
        {
            Validar.ValidarNome(areaDto.NomeArea);

            Area areaBanco = _repository.BuscarPorNome(areaDto.NomeArea);

            if(areaBanco != null)
            {
                throw new DomainException("Ja eciste uma area cadastrada com esse nome.");
            }

            Area area = new Area
            {
                NomeArea = areaDto.NomeArea,
            };

            _repository.Adicionar(area);
        }

        public void Atualizar(Guid id, CriarAreaDto dto)
        {
            Validar.ValidarNome(dto.NomeArea);

            Area areaBanco = _repository.BuscarPorId(id);

            if( areaBanco == null)
            {
                throw new DomainException("Area não encontrada.");
            }

            Area areaExistente = _repository.BuscarPorNome(dto.NomeArea);

            if (areaExistente != null)
            {
                throw new DomainException("Area existente");
            }

            _repository.Atualizar(dto);
        }
    }
}
