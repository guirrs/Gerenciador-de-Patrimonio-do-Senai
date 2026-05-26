using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;
using GestaoPatrimonios_v1.DTOs.StatusTransferenciaDto;

namespace GestaoPatrimonios.Applications.Services
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
            List<StatusTransferencia> statusTransferencia = _repository.Listar();

            List<ListarStatusTransferenciaDto> statusTransferenciaDto = statusTransferencia.Select(status => new ListarStatusTransferenciaDto
            {
                StatusTransferenciaID = status.StatusTransferenciaID,
                NomeStatus = status.NomeStatus
            }).ToList();

            return statusTransferenciaDto;
        }

        public ListarStatusTransferenciaDto BuscarPorId(Guid statusTransferenciaId)
        {
            StatusTransferencia statusTransferencia = _repository.BuscarPorId(statusTransferenciaId);

            if (statusTransferencia == null)
            {
                throw new DomainException("Status de transferência não encontrado.");
            }

            ListarStatusTransferenciaDto statusTransferenciaDto = new ListarStatusTransferenciaDto
            {
                StatusTransferenciaID = statusTransferencia.StatusTransferenciaID,
                NomeStatus = statusTransferencia.NomeStatus
            };

            return statusTransferenciaDto;
        }

        public void Adicionar(CriarStatusTransferenciaDto dto)
        {
            Validar.ValidarNome(dto.NomeStatus);

            StatusTransferencia statusExistente = _repository.BuscarPorNome(dto.NomeStatus);

            if (statusExistente != null)
            {
                throw new DomainException("Já existe um status de transferência cadastrado com esse nome.");
            }

            StatusTransferencia statusTransferencia = new StatusTransferencia
            {
                NomeStatus = dto.NomeStatus
            };

            _repository.Adicionar(statusTransferencia);
        }

        public void Atualizar(Guid statusTransferenciaId, CriarStatusTransferenciaDto dto)
        {
            Validar.ValidarNome(dto.NomeStatus);

            StatusTransferencia statusBanco = _repository.BuscarPorId(statusTransferenciaId);

            if (statusBanco == null)
            {
                throw new DomainException("Status de transferência não encontrado.");
            }

            StatusTransferencia statusExistente = _repository.BuscarPorNome(dto.NomeStatus);

            if (statusExistente != null)
            {
                throw new DomainException("Já existe um status de transferência cadastrado com esse nome.");
            }

            statusBanco.NomeStatus = dto.NomeStatus;

            _repository.Atualizar(statusBanco);
        }
    }
}
