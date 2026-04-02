using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.CidadeDto;
using GestaoPatrimonio.Exceptions;
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
            return _repository.Listar().Select(c => new ListarCidadeDto { 
                NomeCidade = c.NomeCidade,
                Estado = c.Estado,
            }).ToList();
        }

        public ListarCidadeDto ObterPorID(Guid id)
        {
            Cidade? cidade = _repository.ObterPorId(id);

            if (cidade == null)
                throw new DomainException("Cidade não encontrada");

            return CidadeParaDto.ConverterParaDto(cidade);
        }

        public ListarCidadeDto ObterPorNome(string nome)
        {
            Cidade? cidade = _repository.ObterPorNome(nome);

            if (cidade == null)
                throw new DomainException("Cidade não encontrada");

            return CidadeParaDto.ConverterParaDto(cidade);
        }

        public ListarCidadeDto ObterPorNomeEEstado(string nome, string estado)
        {
            Cidade? cidade = _repository.ObterPorNomeEEstado(nome, estado);

            if (cidade == null)
                throw new DomainException("Cidade não encontrada");

            return CidadeParaDto.ConverterParaDto(cidade);  
        }
    }
}
