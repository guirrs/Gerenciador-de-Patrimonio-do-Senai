using GestaoPatrimonios.Contexts;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonios.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public UsuarioRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Usuario> Listar()
        {
            return _context.Usuario
                .Include(usuario => usuario.Cargo)
                .Include(usuario => usuario.TipoUsuario)
                .Include(usuario => usuario.Endereco)
                    .ThenInclude(endereco => endereco.Bairro)
                        .ThenInclude(bairro => bairro.Cidade)
                .OrderBy(usuario => usuario.Nome)
                .ToList();
        }

        public Usuario BuscarPorId(Guid usuarioId)
        {
            return _context.Usuario
                .Include(usuario => usuario.Cargo)
                .Include(usuario => usuario.TipoUsuario)
                .Include(usuario => usuario.Endereco)
                    .ThenInclude(endereco => endereco.Bairro)
                        .ThenInclude(bairro => bairro.Cidade)
                .FirstOrDefault(usuario => usuario.UsuarioID == usuarioId);
        }

        public Usuario BuscarPorNif(string nif)
        {
            return _context.Usuario
                .FirstOrDefault(usuario => usuario.NIF == nif);
        }

        public Usuario BuscarDuplicado(string nif, string cpf, string email, Guid? usuarioId = null)
        {
            var consulta = _context.Usuario.AsQueryable();

            if (usuarioId.HasValue)
            {
                consulta = consulta.Where(usuario => usuario.UsuarioID != usuarioId.Value);
            }

            return consulta.FirstOrDefault(usuario =>
                usuario.NIF == nif ||
                usuario.CPF == cpf ||
                usuario.Email.ToLower() == email.ToLower()
            );
        }

        public bool CargoExiste(Guid cargoId)
        {
            return _context.Cargo.Any(cargo => cargo.CargoID == cargoId);
        }

        public bool TipoUsuarioExiste(Guid tipoUsuarioId)
        {
            return _context.TipoUsuario.Any(tipoUsuario => tipoUsuario.TipoUsuarioID == tipoUsuarioId);
        }

        public void Adicionar(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
        }

        public void Atualizar(Usuario usuario)
        {
            Usuario usuarioBanco = _context.Usuario.Find(usuario.UsuarioID);

            if (usuarioBanco == null)
            {
                return;
            }

            usuarioBanco.NIF = usuario.NIF;
            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.RG = usuario.RG;
            usuarioBanco.CPF = usuario.CPF;
            usuarioBanco.CarteiraTrabalho = usuario.CarteiraTrabalho;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.EnderecoID = usuario.EnderecoID;
            usuarioBanco.CargoID = usuario.CargoID;
            usuarioBanco.TipoUsuarioID = usuario.TipoUsuarioID;

            _context.SaveChanges();
        }

        public void AtualizarStatus(Usuario usuario)
        {
            Usuario usuarioBanco = _context.Usuario.Find(usuario.UsuarioID);

            if (usuarioBanco == null)
            {
                return;
            }

            usuarioBanco.Ativo = usuario.Ativo;
            _context.SaveChanges();
        }

        public Usuario ObterPorNIFComTipoUsuario(string nif)
        {
            return _context.Usuario
                .Include(usuario => usuario.TipoUsuario)
                .FirstOrDefault(usuario => usuario.NIF == nif);
        }

        public void AtualizarSenha(Usuario usuario)
        {
            Usuario usuarioBanco = _context.Usuario.Find(usuario.UsuarioID);

            if (usuarioBanco == null)
            {
                return;
            }

            usuarioBanco.Senha = usuario.Senha;
            _context.SaveChanges();
        }

        public void AtualizarPrimeiroAcesso(Usuario usuario)
        {
            Usuario usuarioBanco = _context.Usuario.Find(usuario.UsuarioID);

            if (usuarioBanco == null)
            {
                return;
            }

            usuarioBanco.PrimeiroAcesso = usuario.PrimeiroAcesso;
            _context.SaveChanges();
        }
    }
}