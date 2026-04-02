using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Aplication.Regras;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.EnderecoDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;
using GestaoPatrimonio.Repositories;

namespace GestaoPatrimonio.Aplication.Services
{
    public class EnderecoService
    {
        private readonly IEnderecoRepository _repository;

        public EnderecoService(IEnderecoRepository repository)
        {
            _repository = repository;
        }

        public List<ListarEnderecoDto> Listar ()
        {
            return _repository.Listar().Select(e => EnderecoParaDto.EnderecoListarParaDto(e)).ToList();
        }

        public ListarEnderecoDto ObterPorId(Guid id)
        {
            Endereco? endereco = _repository.ObterPorId(id);

            if(endereco == null)
            {
                throw new DomainException("Endereco não encontrado");
            }

            return EnderecoParaDto.EnderecoListarParaDto(endereco);
        }

        public void Adicionar(CriarEnderecoDto dto)
        {
            Validar.ValidarNome(dto.Logradouro);

            if (!_repository.BairroExiste(dto.BairroID))
                throw new DomainException("Bairro não existe.");

            _repository.Adicionar(EnderecoParaDto.DtoParaDomain(dto, null));
        }

        public void Atualizar(CriarEnderecoDto dto,Guid id)
        {
            Validar.ValidarNome(dto.Logradouro);

            Endereco enderecoBanco = _repository.ObterPorId(id);

            if (enderecoBanco == null)
                throw new DomainException("endereco não encontrado");

            if (!_repository.BairroExiste(dto.BairroID))
                throw new DomainException("Bairro não existe.");

            _repository.Atualizar(EnderecoParaDto.DtoParaDomain(dto, id));
        }
    }
}
