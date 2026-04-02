using GestaoPatrimonio.Aplication.Conversoes;
using GestaoPatrimonio.Aplication.Regras;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.BairroDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;
using GestaoPatrimonio.Repositories;

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
            Validar.ValidarNome(dto.NomeBairro);

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
            Validar.ValidarNome(dto.NomeBairro);

            Bairro nomeBairro = _repository.BuscarPorNome(dto.NomeBairro, dto.CidadeID);
            Bairro bairroBanco = _repository.BuscarPorId(id);

            if (nomeBairro != null)
                throw new DomainException("Bairro ja cadastrado");

            _repository.Atualizar(BairroParaDto.DtoParaDomain(dto,id));
        }
    }
}
