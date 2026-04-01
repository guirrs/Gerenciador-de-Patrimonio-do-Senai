using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.StatusTransferencia;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class StatusTransferenciaService
    {
        private readonly IStatusTransferenciaRepository _repository;

        public StatusTransferenciaService(IStatusTransferenciaRepository repository)
        {
            _repository = repository;
        }

        public List<ListarStatusTransferenciaDto> Listar()
        {
            return _repository.Listar().Select(s => StatusTransferenciaParaDto.StatusListarParaDto(s)).ToList();
        }

        public ListarStatusTransferenciaDto ObterPorId(Guid id)
        {
            StatusTransferencia status = _repository.ObterPorId(id);

            if (status == null)
                throw new DomainException("Id não encontrado.");

            return StatusTransferenciaParaDto.StatusListarParaDto(status);
        }

        public ListarStatusTransferenciaDto ObterPorNome(string nome)
        {
            StatusTransferencia status = _repository.ObterPorNome(nome);

            if (status == null)
                throw new DomainException("Nome não encontrado");

            return StatusTransferenciaParaDto.StatusListarParaDto(status);
        }

        public void Adicionar(CriarStatusTransferenciaDto dto)
        {
            if (_repository.ObterPorNome(dto.NomeStatus) != null)
                throw new DomainException("Nome ja cadastrado");

            StatusTransferencia status = new StatusTransferencia
            {
                NomeStatus = dto.NomeStatus
            };

            _repository.Adicionar(status);
        }

        public void Atualizar(CriarStatusTransferenciaDto dto, Guid id)
        {
            if (_repository.ObterPorNome(dto.NomeStatus) != null)
                throw new DomainException("Nome ja cadastrado");

            StatusTransferencia statusBanco = _repository.ObterPorId(id);

            statusBanco.NomeStatus = dto.NomeStatus;

            _repository.Atualizar(statusBanco);
        }
    }
}
