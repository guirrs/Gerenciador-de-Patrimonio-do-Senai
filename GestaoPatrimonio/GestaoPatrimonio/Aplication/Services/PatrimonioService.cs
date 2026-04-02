using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Aplication.Regras;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.PatrimonioDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Repositories;

namespace GestaoPatrimonio.Aplication.Services
{
    public class PatrimonioService
    {
        private readonly PatrimonioRepository _repository;

        public PatrimonioService(PatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarPatrimonioDto> Listar()
        {
            return _repository.Listar().Select(l => PatrimonioParaDto.ListarPatrimonioParaDto(l)).ToList();
        }

        public ListarPatrimonioDto ObterPorId(Guid id)
        {
            Patrimonio patrimonio = _repository.ObterPorId(id);

            if (patrimonio == null)
                throw new DomainException("Patromonio não encontrado.");

            return PatrimonioParaDto.ListarPatrimonioParaDto(patrimonio);
        }

        public void Adicionar(CriarPatrimonioDto dto)
        {
            Validar.ValidarNome(dto.Denominacao);

            Patrimonio patrimonioNome = _repository.ObterPorDenominacao(dto.Denominacao);

            if(patrimonioNome != null)
            {
                throw new DomainException("Nome ja cadastrado");
            }

            _repository.Adicionar(PatrimonioParaDto.DtoParaDomain(dto,null));
        }

        public void Atualizar(CriarPatrimonioDto dto, Guid id)
        {
            Validar.ValidarNome(dto.Denominacao);

            Patrimonio patrimonioNome = _repository.ObterPorDenominacao(dto.Denominacao);

            if (patrimonioNome != null)
            {
                throw new DomainException("Nome ja cadastrado");
            }

            Patrimonio patrimonio = _repository.ObterPorId(id);

            _repository.Atualizar(PatrimonioParaDto.DtoParaDomain(dto, id));
        }

        public void Remover(Guid id)
        {
            Patrimonio patrimonio = _repository.ObterPorId(id);

            if (patrimonio == null)
                throw new DomainException("Patrimonio nao encontrado");

            _repository.Remover(id);
        }
    }
}
