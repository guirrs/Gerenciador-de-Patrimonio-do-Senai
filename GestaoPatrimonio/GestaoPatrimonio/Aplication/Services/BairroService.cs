using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.BairroDto;
using GestaoPatrimonio.Interface;
using GestaoPatrimonio.Repositories;
using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Exceptions;

namespace GestaoPatrimonio.Aplication.Services
{
    public class BairroService
    {
        private readonly IBairroRepository _repository;

        public BairroService(IBairroRepository repository)
        {
            _repository = repository; 
        }

        public List<ListarBairroDto> Listar()
        {
            return _repository.Listar().Select(b => BairroParaDto.ListarBairroParaDto(b)).ToList();
        }

        public ListarBairroDto BuscarPorId(Guid id)
        {
            Bairro? bairro = _repository.BuscarPorId(id);   
            
            if(bairro == null)
            {
                throw new DomainException("Bairro nao encontrado");
            }

            return BairroParaDto.ListarBairroParaDto(bairro);
        }

        public void Adicionar(CriarBairroDto dto)
        {
            Bairro nomeBairro = _repository.BuscarPorNome(dto.NomeBairro, dto.CidadeID);
            bool cidade = _repository.CidadeExiste(dto.CidadeID);

            if (nomeBairro != null)
                throw new DomainException("Bairro ja cadastrado");

            if (cidade == false)
                throw new DomainException("Essa cidade nao existe");

            Bairro bairro = new Bairro
            {
                CidadeID = dto.CidadeID,
                NomeBairro = dto.NomeBairro,
            };
            _repository.Adicionar(bairro);
        }

        public void Atualizar(CriarBairroDto dto, Guid id)
        {
            Bairro nomeBairro = _repository.BuscarPorNome(dto.NomeBairro, dto.CidadeID);
            Bairro bairroBanco = _repository.BuscarPorId(id);

            if (nomeBairro == null)
                throw new DomainException("Bairro ja cadastrado");

            Bairro bairro = new Bairro
            {
                CidadeID = bairroBanco.CidadeID,
                NomeBairro = bairroBanco.NomeBairro,
            };
            _repository.Atualizar(bairro);
        }
    }
}
