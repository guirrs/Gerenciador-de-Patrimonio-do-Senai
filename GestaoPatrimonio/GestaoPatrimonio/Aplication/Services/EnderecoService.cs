using GestaoPatrimonio.Aplication.Conversoes;
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
            if (!_repository.BairroExiste(dto.BairroID))
                throw new DomainException("Bairro não existe.");

            Endereco endereco = new Endereco
            {
                BairroID = dto.BairroID,
                CEP = dto.CEP,
                Complemento = dto.Complemento,
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
            };

            _repository.Adicionar(endereco);
        }

        public void Atualizar(CriarEnderecoDto dto,Guid id)
        {
            Endereco enderecoBanco = _repository.ObterPorId(id);

            if (!_repository.BairroExiste(enderecoBanco.BairroID))
                throw new DomainException("Bairro não existe.");

            Endereco endereco = new Endereco
            {
                BairroID = enderecoBanco.BairroID,
                CEP = enderecoBanco.CEP,
                Complemento = enderecoBanco.Complemento,
                Logradouro = enderecoBanco.Logradouro,
                Numero = enderecoBanco.Numero,
            };

            _repository.Adicionar(endereco);
        }
    }
}
