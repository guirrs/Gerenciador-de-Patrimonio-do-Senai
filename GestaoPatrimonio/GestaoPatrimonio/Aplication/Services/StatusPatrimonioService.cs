using GestaoPatrimonio.DTOs.StatusPatrimonio;
using GestaoPatrimonio.Repositories;
using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Aplication.Services
{
    public class StatusPatrimonioService
    {
        private readonly StatusPatrimonioRepository _repository;

        public StatusPatrimonioService(StatusPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarStatusPatrimonioDto> Listar()
        {
            return _repository.Listar().Select(s => StatusPatrimonioParaDto.ListarStatusPatrimonioParaDto(s)).ToList();
        }

        public ListarStatusPatrimonioDto ObterPorId(Guid id)
        {
            StatusPatrimonio status = _repository.ObterPorId(id);

            if(status == null)
            {
                throw new DomainException("Status não encontrado.");
            }

            return StatusPatrimonioParaDto.ListarStatusPatrimonioParaDto(status);
        }

        public void Adicionar(CriarStatusPatrimonioDto dto)
        {
            _repository.Adicionar(StatusPatrimonioParaDto.DtoParaDomain(dto,null));
        }
        
        public void Atualizar (CriarStatusPatrimonioDto dto, Guid id)
        {
            StatusPatrimonio status = _repository.ObterPorId(id);

            if (status == null)
                throw new DomainException("Id não encontrado");

            _repository.Atualizar(StatusPatrimonioParaDto.DtoParaDomain(dto, id));
        }
    }
}
