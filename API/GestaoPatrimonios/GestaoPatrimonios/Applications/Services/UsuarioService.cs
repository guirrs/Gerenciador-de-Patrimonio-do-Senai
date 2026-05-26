using GestaoPatrimonios.Applications.Autenticacao;
using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.DTOs.UsuarioDto;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;

namespace GestaoPatrimonios.Applications.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICidadeRepository _cidadeRepository;
        private readonly IBairroRepository _bairroRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            ICidadeRepository cidadeRepository,
            IBairroRepository bairroRepository,
            IEnderecoRepository enderecoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _cidadeRepository = cidadeRepository;
            _bairroRepository = bairroRepository;
            _enderecoRepository = enderecoRepository;
        }

        public List<ListarUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _usuarioRepository.Listar();

            return usuarios.Select(usuario => ConverterParaDto(usuario)).ToList();
        }

        public ListarUsuarioDto BuscarPorId(Guid usuarioId)
        {
            Usuario usuario = _usuarioRepository.BuscarPorId(usuarioId);

            if (usuario == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            return ConverterParaDto(usuario);
        }

        public void Adicionar(CriarUsuarioDto dto)
        {
            ValidarDadosUsuario(dto);
            ValidarUsuarioDuplicado(dto);

            if (!_usuarioRepository.CargoExiste(dto.CargoID))
            {
                throw new DomainException("Cargo informado não existe.");
            }

            if (!_usuarioRepository.TipoUsuarioExiste(dto.TipoUsuarioID))
            {
                throw new DomainException("Tipo de usuário informado não existe.");
            }

            Endereco endereco = ObterOuCadastrarEndereco(dto);

            Usuario usuario = new Usuario
            {
                NIF = dto.NIF,
                Nome = dto.Nome,
                RG = dto.RG,
                CPF = dto.CPF,
                CarteiraTrabalho = dto.CarteiraTrabalho,
                Senha = CriptografiaUsuario.CriptografarSenha(dto.NIF),
                Email = dto.Email,
                Ativo = true,
                PrimeiroAcesso = true,
                EnderecoID = endereco.EnderecoID,
                CargoID = dto.CargoID,
                TipoUsuarioID = dto.TipoUsuarioID
            };

            _usuarioRepository.Adicionar(usuario);
        }

        public void Atualizar(Guid usuarioId, CriarUsuarioDto dto)
        {
            ValidarDadosUsuario(dto);

            Usuario usuarioBanco = _usuarioRepository.BuscarPorId(usuarioId);

            if (usuarioBanco == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            ValidarUsuarioDuplicado(dto, usuarioId);

            if (!_usuarioRepository.CargoExiste(dto.CargoID))
            {
                throw new DomainException("Cargo informado não existe.");
            }

            if (!_usuarioRepository.TipoUsuarioExiste(dto.TipoUsuarioID))
            {
                throw new DomainException("Tipo de usuário informado não existe.");
            }

            Endereco endereco = ObterOuCadastrarEndereco(dto);

            usuarioBanco.NIF = dto.NIF;
            usuarioBanco.Nome = dto.Nome;
            usuarioBanco.RG = dto.RG;
            usuarioBanco.CPF = dto.CPF;
            usuarioBanco.CarteiraTrabalho = dto.CarteiraTrabalho;
            usuarioBanco.Email = dto.Email;
            usuarioBanco.EnderecoID = endereco.EnderecoID;
            usuarioBanco.CargoID = dto.CargoID;
            usuarioBanco.TipoUsuarioID = dto.TipoUsuarioID;

            _usuarioRepository.Atualizar(usuarioBanco);
        }

        public void AtualizarStatus(Guid usuarioId, AtualizarStatusUsuarioDto dto)
        {
            Usuario usuarioBanco = _usuarioRepository.BuscarPorId(usuarioId);

            if (usuarioBanco == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            usuarioBanco.Ativo = dto.Ativo;

            _usuarioRepository.AtualizarStatus(usuarioBanco);
        }

        private void ValidarDadosUsuario(CriarUsuarioDto dto)
        {
            Validar.ValidarNome(dto.Nome);
            Validar.ValidarNIF(dto.NIF);
            Validar.ValidarCPF(dto.CPF);
            Validar.ValidarEmail(dto.Email);

            if (string.IsNullOrWhiteSpace(dto.Logradouro))
            {
                throw new DomainException("Logradouro é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(dto.NomeBairro))
            {
                throw new DomainException("Bairro é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(dto.NomeCidade))
            {
                throw new DomainException("Cidade é obrigatória.");
            }

            if (string.IsNullOrWhiteSpace(dto.Estado))
            {
                throw new DomainException("Estado é obrigatório.");
            }
        }

        private void ValidarUsuarioDuplicado(CriarUsuarioDto dto, Guid? usuarioId = null)
        {
            Usuario usuarioDuplicado = _usuarioRepository.BuscarDuplicado(
                dto.NIF,
                dto.CPF,
                dto.Email,
                usuarioId
            );

            if (usuarioDuplicado == null)
            {
                return;
            }

            if (usuarioDuplicado.NIF == dto.NIF)
            {
                throw new DomainException("Já existe um usuário cadastrado com esse NIF.");
            }

            if (usuarioDuplicado.CPF == dto.CPF)
            {
                throw new DomainException("Já existe um usuário cadastrado com esse CPF.");
            }

            if (usuarioDuplicado.Email.ToLower() == dto.Email.ToLower())
            {
                throw new DomainException("Já existe um usuário cadastrado com esse e-mail.");
            }
        }

        private Endereco ObterOuCadastrarEndereco(CriarUsuarioDto dto)
        {
            Cidade cidade = ObterOuCadastrarCidade(dto.NomeCidade, dto.Estado);
            Bairro bairro = ObterOuCadastrarBairro(dto.NomeBairro, cidade.CidadeID);

            Endereco endereco = _enderecoRepository.BuscarPorLogradouroENumero(
                dto.Logradouro,
                dto.Numero,
                bairro.BairroID
            );

            if (endereco != null)
            {
                return endereco;
            }

            endereco = new Endereco
            {
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                CEP = dto.CEP,
                BairroID = bairro.BairroID
            };

            _enderecoRepository.Adicionar(endereco);

            return endereco;
        }

        private Cidade ObterOuCadastrarCidade(string nomeCidade, string estado)
        {
            Cidade cidade = _cidadeRepository.BuscarPorNomeEEstado(nomeCidade, estado);

            if (cidade != null)
            {
                return cidade;
            }

            cidade = new Cidade
            {
                NomeCidade = nomeCidade,
                Estado = estado
            };

            _cidadeRepository.Adicionar(cidade);

            return cidade;
        }

        private Bairro ObterOuCadastrarBairro(string nomeBairro, Guid cidadeId)
        {
            Bairro bairro = _bairroRepository.BuscarPorNome(nomeBairro, cidadeId);

            if (bairro != null)
            {
                return bairro;
            }

            bairro = new Bairro
            {
                NomeBairro = nomeBairro,
                CidadeID = cidadeId
            };

            _bairroRepository.Adicionar(bairro);

            return bairro;
        }

        private ListarUsuarioDto ConverterParaDto(Usuario usuario)
        {
            return new ListarUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                NIF = usuario.NIF,
                Nome = usuario.Nome,
                RG = usuario.RG,
                CPF = usuario.CPF,
                CarteiraTrabalho = usuario.CarteiraTrabalho,
                Email = usuario.Email,
                Ativo = usuario.Ativo,
                PrimeiroAcesso = usuario.PrimeiroAcesso,

                CargoID = usuario.CargoID,
                NomeCargo = usuario.Cargo?.NomeCargo ?? string.Empty,

                TipoUsuarioID = usuario.TipoUsuarioID,
                NomeTipoUsuario = usuario.TipoUsuario?.NomeTipo ?? string.Empty,

                EnderecoID = usuario.EnderecoID,
                Logradouro = usuario.Endereco?.Logradouro ?? string.Empty,
                Numero = usuario.Endereco?.Numero,
                Complemento = usuario.Endereco?.Complemento,
                CEP = usuario.Endereco?.CEP,

                NomeBairro = usuario.Endereco?.Bairro?.NomeBairro ?? string.Empty,
                NomeCidade = usuario.Endereco?.Bairro?.Cidade?.NomeCidade ?? string.Empty,
                Estado = usuario.Endereco?.Bairro?.Cidade?.Estado ?? string.Empty
            };
        }
    }
}