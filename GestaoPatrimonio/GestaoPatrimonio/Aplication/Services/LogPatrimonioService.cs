using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.DTOs.LogPatrimonioDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class LogPatrimonioService
    {
        private readonly ILogPatrimonioRepository _repository;

        public LogPatrimonioService(ILogPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarLogPatrimonioDto> Listar()
        {
            return _repository.Listar().Select(l => LogPatrimonioParaDto.ConverterParaDto(l)).ToList();
        }

        public List<ListarLogPatrimonioDto> ListarPorPatrimonio(Guid patrimonioId)
        {
            List<ListarLogPatrimonioDto> lista = _repository.BuscarPorPatrimonio(patrimonioId).Select(l => LogPatrimonioParaDto.ConverterParaDto(l)).ToList();
            if (lista.Count == 0)
                throw new DomainException("Log não existe.");
            return lista;
        }
    }
}
