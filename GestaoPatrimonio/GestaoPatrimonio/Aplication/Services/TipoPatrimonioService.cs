using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.TipoPatromonio;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class TipoPatrimonioService
    {
        private readonly ITipoPatrimonioRepository _repository;

        public TipoPatrimonioService(ITipoPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarTipoPatrimonioDto> Listar()
        {
            return _repository.Listar().Select(l => TipoPatrimonioParaDto.TipoPatrimonioListarParaDto(l)).ToList();
        }

        public ListarTipoPatrimonioDto ObterPorId(Guid id)
        {
            TipoPatrimonio tipo = _repository.ObterPorId(id);
            if (tipo == null)
                throw new DomainException("ID não encontrado");

            return TipoPatrimonioParaDto.TipoPatrimonioListarParaDto(tipo);
        }

        public void Adicionar(CriarTipoPatrimonioDto dto)
        {
            if (_repository.ObterPorNome(dto.NomeTipo) != null)
                throw new DomainException("Tipo patrimonio ja cadastrado");

            TipoPatrimonio tipo = new TipoPatrimonio
            {
                NomeTipo = dto.NomeTipo
            };

            _repository.Adicionar(tipo);
        }

        public void Atualizar(CriarTipoPatrimonioDto dto, Guid id)
        {
            if (_repository.ObterPorNome(dto.NomeTipo) != null)
                throw new DomainException("Tipo patrimonio ja cadastrado");

            TipoPatrimonio tipo = _repository.ObterPorId(id);

            tipo.NomeTipo = dto.NomeTipo;

            _repository.Atualizar(tipo);
        }
    }
}
