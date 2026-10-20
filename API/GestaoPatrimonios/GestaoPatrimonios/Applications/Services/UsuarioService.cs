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
            return _usuarioRepository.Listar()
                .Select(ConverterParaDto)
                .ToList();
        }

        public ListarUsuarioDto BuscarPorId(Guid usuarioId)
        {
            return ConverterParaDto(ObterUsuario(usuarioId));
        }

        public void Adicionar(CriarUsuarioDto dto)
        {
            ValidarDadosUsuario(dto);
            ValidarUsuarioDuplicado(dto);
            ValidarCargoETipoUsuario(dto);

            Endereco endereco = ObterOuCadastrarEndereco(dto);

            Usuario usuario = new Usuario
            {
                Senha = CriptografiaUsuario.CriptografarSenha(dto.NIF),
                Ativo = true,
                PrimeiroAcesso = true
            };

            PreencherDadosUsuario(usuario, dto, endereco);

            _usuarioRepository.Adicionar(usuario);
        }

        public void Atualizar(Guid usuarioId, CriarUsuarioDto dto)
        {
            ValidarDadosUsuario(dto);

            Usuario usuarioBanco = ObterUsuario(usuarioId);

            ValidarUsuarioDuplicado(dto, usuarioId);
            ValidarCargoETipoUsuario(dto);

            Endereco endereco = ObterOuCadastrarEndereco(dto);

            PreencherDadosUsuario(usuarioBanco, dto, endereco);

            _usuarioRepository.Atualizar(usuarioBanco);
        }

        public void AtualizarStatus(Guid usuarioId, AtualizarStatusUsuarioDto dto)
        {
            Usuario usuarioBanco = ObterUsuario(usuarioId);

            usuarioBanco.Ativo = dto.Ativo;

            _usuarioRepository.AtualizarStatus(usuarioBanco);
        }

        // ======================= Validações =======================

        private static void ValidarDadosUsuario(CriarUsuarioDto dto)
        {
            Validar.ValidarNome(dto.Nome);
            Validar.ValidarNIF(dto.NIF);
            Validar.ValidarCPF(dto.CPF);
            Validar.ValidarEmail(dto.Email);

            ExigirCampo(dto.Logradouro, "Logradouro é obrigatório.");
            ExigirCampo(dto.NomeBairro, "Bairro é obrigatório.");
            ExigirCampo(dto.NomeCidade, "Cidade é obrigatória.");
            ExigirCampo(dto.Estado, "Estado é obrigatório.");
        }

        private static void ExigirCampo(string? valor, string mensagem)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException(mensagem);
        }

        private void ValidarCargoETipoUsuario(CriarUsuarioDto dto)
        {
            if (!_usuarioRepository.CargoExiste(dto.CargoID))
                throw new DomainException("Cargo informado não existe.");

            if (!_usuarioRepository.TipoUsuarioExiste(dto.TipoUsuarioID))
                throw new DomainException("Tipo de usuário informado não existe.");
        }

        private void ValidarUsuarioDuplicado(CriarUsuarioDto dto, Guid? usuarioId = null)
        {
            Usuario? duplicado = _usuarioRepository.BuscarDuplicado(
                dto.NIF,
                dto.CPF,
                dto.Email,
                usuarioId
            );

            if (duplicado == null)
                return;

            if (duplicado.NIF == dto.NIF)
                throw new DomainException("Já existe um usuário cadastrado com esse NIF.");

            if (duplicado.CPF == dto.CPF)
                throw new DomainException("Já existe um usuário cadastrado com esse CPF.");

            if (string.Equals(duplicado.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
                throw new DomainException("Já existe um usuário cadastrado com esse e-mail.");
        }

        // ======================= Busca / mapeamento =======================

        private Usuario ObterUsuario(Guid usuarioId)
        {
            return _usuarioRepository.BuscarPorId(usuarioId)
                ?? throw new DomainException("Usuário não encontrado.");
        }

        // Campos em comum entre cadastro e atualização (evita duplicar atribuições)
        private static void PreencherDadosUsuario(Usuario usuario, CriarUsuarioDto dto, Endereco endereco)
        {
            usuario.NIF = dto.NIF;
            usuario.Nome = dto.Nome;
            usuario.RG = dto.RG;
            usuario.CPF = dto.CPF;
            usuario.CarteiraTrabalho = dto.CarteiraTrabalho;
            usuario.Email = dto.Email;
            usuario.EnderecoID = endereco.EnderecoID;
            usuario.CargoID = dto.CargoID;
            usuario.TipoUsuarioID = dto.TipoUsuarioID;
        }

        private static ListarUsuarioDto ConverterParaDto(Usuario usuario)
        {
            Endereco? endereco = usuario.Endereco;
            Cidade? cidade = endereco?.Bairro?.Cidade;

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
                Logradouro = endereco?.Logradouro ?? string.Empty,
                Numero = endereco?.Numero,
                Complemento = endereco?.Complemento,
                CEP = endereco?.CEP,

                NomeBairro = endereco?.Bairro?.NomeBairro ?? string.Empty,
                NomeCidade = cidade?.NomeCidade ?? string.Empty,
                Estado = cidade?.Estado ?? string.Empty
            };
        }

        // ======================= Endereço (obter ou cadastrar) =======================

        private Endereco ObterOuCadastrarEndereco(CriarUsuarioDto dto)
        {
            Cidade cidade = ObterOuCadastrarCidade(dto.NomeCidade.Trim(), dto.Estado.Trim());
            Bairro bairro = ObterOuCadastrarBairro(dto.NomeBairro.Trim(), cidade.CidadeID);

            string logradouro = dto.Logradouro.Trim();

            Endereco? endereco = _enderecoRepository.BuscarPorLogradouroENumero(
                logradouro,
                dto.Numero,
                bairro.BairroID
            );

            if (endereco != null)
                return endereco;

            endereco = new Endereco
            {
                Logradouro = logradouro,
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
            Cidade? cidade = _cidadeRepository.BuscarPorNomeEEstado(nomeCidade, estado);

            if (cidade != null)
                return cidade;

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
            Bairro? bairro = _bairroRepository.BuscarPorNome(nomeBairro, cidadeId);

            if (bairro != null)
                return bairro;

            bairro = new Bairro
            {
                NomeBairro = nomeBairro,
                CidadeID = cidadeId
            };

            _bairroRepository.Adicionar(bairro);

            return bairro;
        }
    }
}
