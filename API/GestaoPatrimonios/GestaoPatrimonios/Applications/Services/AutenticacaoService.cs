using System.Security.Cryptography;
using GestaoPatrimonios.Applications.Autenticacao;
using GestaoPatrimonios.Applications.Regras;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.DTOs.AutenticacaoDto;
using GestaoPatrimonios.Exceptions;
using GestaoPatrimonios.Interfaces;

namespace GestaoPatrimonios.Applications.Services
{
    public class AutenticacaoService
    {
        private const string MensagemCredenciaisInvalidas = "NIF ou senha inválidos.";

        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        public TokenDto Login(LoginDto loginDto)
        {
            Usuario? usuario = _repository.ObterPorNIFComTipoUsuario(loginDto.NIF);

            // Sempre calcula o hash, mesmo quando o usuário não existe: o tempo de
            // resposta fica parecido e não revela quais NIFs estão cadastrados.
            bool senhaValida = VerificarSenha(loginDto.Senha, usuario?.Senha);

            if (usuario == null || !senhaValida)
                throw new DomainException(MensagemCredenciaisInvalidas);

            // Só informa que o usuário está inativo depois de a senha estar correta,
            // para não permitir descobrir contas existentes sem conhecer a senha.
            if (!usuario.Ativo)
                throw new DomainException("Usuário inativo.");

            return new TokenDto
            {
                Token = _tokenJwt.GerarToken(usuario),
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                TipoUsuario = usuario.TipoUsuario.NomeTipo
            };
        }

        public void TrocarPrimeiraSenha(Guid usuarioId, TrocarPrimeiraSenhaDto dto)
        {
            Validar.ValidarSenha(dto.SenhaAtual);
            Validar.ValidarSenha(dto.NovaSenha);

            Usuario usuario = _repository.BuscarPorId(usuarioId)
                ?? throw new DomainException("Usuário não encontrado.");

            if (!VerificarSenha(dto.SenhaAtual, usuario.Senha))
                throw new DomainException("Senha atual inválida.");

            if (dto.SenhaAtual == dto.NovaSenha)
                throw new DomainException("A nova senha deve ser diferente da senha atual.");

            usuario.Senha = CriptografiaUsuario.CriptografarSenha(dto.NovaSenha);
            usuario.PrimeiroAcesso = false;

            _repository.AtualizarSenha(usuario);
            _repository.AtualizarPrimeiroAcesso(usuario);
        }

        private static bool VerificarSenha(string senhaDigitada, byte[]? senhaHashBanco)
        {
            byte[] hashDigitado = CriptografiaUsuario.CriptografarSenha(senhaDigitada);

            if (senhaHashBanco == null)
                return false;

            // Comparação em tempo constante (SequenceEqual sai no primeiro byte diferente)
            return CryptographicOperations.FixedTimeEquals(hashDigitado, senhaHashBanco);
        }
    }
}
