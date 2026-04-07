using GestaoPatrimonio.Aplication.Autenticacao;
using GestaoPatrimonio.Aplication.Regras;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.AutenticacaoDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

namespace GestaoPatrimonio.Aplication.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        private static bool VerificarSenha(string senhaDigitada, byte[] senhaHashBanco)
        {
            var hashDigitado = CriptografiaUsuario.CriptografiaSenha(senhaDigitada);

            return hashDigitado.SequenceEqual(senhaHashBanco);
        }

        public TokenDto Login(LoginDto loginDto)
        {
            Usuario usuario = _repository.ObterPorNIFComTipoUsuario(loginDto.NIF);

            if(usuario == null)
            {
                throw new DomainException("NIF ou senha inválidos.");
            }

            if(usuario.Ativo == false)
            {
                throw new DomainException("Usuário inativo.");
            }

            if(!VerificarSenha(loginDto.Senha, usuario.Senha))
            {
                throw new DomainException("NIF ou senha inválidos");
            }

            string token = _tokenJwt.GerarToken(usuario);

            TokenDto novoToken = new TokenDto
            {
                Token = token,
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                TipoUsuario = usuario.TipoUsuario.NomeTipo,
            };

            return novoToken;   
        }

        public void TrocarPrimeiraSenha(Guid id, TrocarPrimeiroSenhaDto dto)
        {
            Validar.ValidarSenha(dto.SenhaAtual);
            Validar.ValidarSenha(dto.NovaSenha);

            Usuario usuario = _repository.ObterPorId(id);

            if(usuario == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            if (!VerificarSenha(dto.SenhaAtual, usuario.Senha))
                throw new DomainException("Senha atual invalida.");

            if(dto.SenhaAtual == dto.NovaSenha)
            {
                throw new DomainException("A nova senha deve ser diferente da senha atual.");
            }

            _repository.AtualizarSenha(id, dto.NovaSenha);
            _repository.AtualizarPrimeiroAcesso(id, false);
        }
    }
}
