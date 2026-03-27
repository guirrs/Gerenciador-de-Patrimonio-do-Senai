using GestaoPatrimonio.DTOs.CidadeDto;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class CidadeService
    {
        private readonly ICidadeRepository _repository;

        public CidadeService(ICidadeRepository repository)
        {
            _repository = repository;
        }

        public List<ListarCidadeDto> Listar()
        {
            return _repository.Listar().Select(c => new CriarCidadeDto);
        }
    }
}
