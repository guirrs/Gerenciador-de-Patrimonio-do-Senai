//using GestaoPatrimonios.Applications.Regras;
//using GestaoPatrimonios.Domains;
//using GestaoPatrimonios.Exceptions;
//using GestaoPatrimonios.Interfaces;
//using GestaoPatrimonios_v1.DTOs.EnderecoDto;

//namespace GestaoPatrimonios.Applications.Services
//{
//    public class EnderecoService
//    {
//        private readonly IEnderecoRepository _repository;

//        public EnderecoService(IEnderecoRepository repository)
//        {
//            _repository = repository;
//        }

//        public List<ListarEnderecoDto> Listar()
//        {
//            List<Endereco> enderecos = _repository.Listar();

//            List<ListarEnderecoDto> enderecosDto = enderecos.Select(endereco => new ListarEnderecoDto
//            {
//                EnderecoID = endereco.EnderecoID,
//                Logradouro = endereco.Logradouro,
//                Numero = endereco.Numero,
//                Complemento = endereco.Complemento,
//                BairroID = endereco.BairroID
//            }).ToList();

//            return enderecosDto;
//        }

//        public ListarEnderecoDto BuscarPorId(Guid enderecoId)
//        {
//            Endereco endereco = _repository.BuscarPorId(enderecoId);

//            if (endereco == null)
//            {
//                throw new DomainException("Endereço não encontrado.");
//            }

//            return new ListarEnderecoDto
//            {
//                EnderecoID = endereco.EnderecoID,
//                Logradouro = endereco.Logradouro,
//                Numero = endereco.Numero,
//                Complemento = endereco.Complemento,
//                BairroID = endereco.BairroID
//            };
//        }

//        public void Adicionar(CriarEnderecoDto dto)
//        {
//            Validar.ValidarNome(dto.Logradouro);

//            if (!_repository.BairroExiste(dto.BairroID))
//            {
//                throw new DomainException("Bairro informado não existe.");
//            }

//            Endereco enderecoExistente = _repository.BuscarPorLogradouroENumero(
//                dto.Logradouro,
//                dto.Numero,
//                dto.BairroID
//            );

//            if (enderecoExistente != null)
//            {
//                throw new DomainException("Já existe um endereço com esses dados.");
//            }

//            Endereco endereco = new Endereco
//            {
//                Logradouro = dto.Logradouro,
//                Numero = dto.Numero,
//                Complemento = dto.Complemento,
//                BairroID = dto.BairroID
//            };

//            _repository.Adicionar(endereco);
//        }

//        public void Atualizar(Guid enderecoId, CriarEnderecoDto dto)
//        {
//            Validar.ValidarNome(dto.Logradouro);

//            Endereco enderecoBanco = _repository.BuscarPorId(enderecoId);

//            if (enderecoBanco == null)
//            {
//                throw new DomainException("Endereço não encontrado.");
//            }

//            if (!_repository.BairroExiste(dto.BairroID))
//            {
//                throw new DomainException("Bairro informado não existe.");
//            }

//            Endereco enderecoExistente = _repository.BuscarPorLogradouroENumero(
//                dto.Logradouro,
//                dto.Numero,
//                dto.BairroID,
//                enderecoId
//            );

//            if (enderecoExistente != null)
//            {
//                throw new DomainException("Já existe um endereço com esses dados.");
//            }

//            enderecoBanco.Logradouro = dto.Logradouro;
//            enderecoBanco.Numero = dto.Numero;
//            enderecoBanco.Complemento = dto.Complemento;
//            enderecoBanco.BairroID = dto.BairroID;

//            _repository.Atualizar(enderecoBanco);
//        }
//    }
//}
