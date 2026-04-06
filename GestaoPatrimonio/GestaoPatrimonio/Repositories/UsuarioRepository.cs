using GestaoPatrimonio.Aplication.Autenticacao;
using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Repositories
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
            return _context.Usuario.AsNoTracking().OrderBy(u => u.Nome)
                .Include(u => u.Endereco)
                .Include(u => u.CargoID)
                .Include(u => u.TipoUsuario)
                .ToList();
        }

        public Usuario ObterPorId(Guid id)
        {
            return _context.Usuario.Find(id);
        }

        public Usuario BuscarDuplicado(string nif, string cpf, string email, Guid? usuarioId = null)
        {
            var consulta = _context.Usuario.AsQueryable();

            if(usuarioId.HasValue)
                consulta = consulta.Where(usuario => usuario.UsuarioID != usuarioId.Value);

            return consulta.FirstOrDefault(usuario =>
            usuario.NIF == nif ||
            usuario.CPF == cpf ||
            usuario.Email.ToLower() == email.ToLower());
        }

        public bool CargoExiste(Guid id)
        {
            return _context.Cargo.Any(c => c.CargoID == id);
        }

        public bool EnderecoExiste(Guid id)
        {
            return _context.Endereco.Any(e => e.EnderecoID == id);
        }

        public bool TipoUsuarioExiste(Guid id)
        {
            return _context.TipoUsuario.Any(t => t.TipoUsuarioID == id);
        }

        public void Adicionar(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
        }

        public void Atualizar(Usuario usuario)
        {
            if (usuario == null)
                return;

            Usuario usuarioBanco = _context.Usuario.Find(usuario.UsuarioID);

            if (usuarioBanco == null)
                return;

            usuarioBanco.CarteiraTrabalho = usuario.CarteiraTrabalho;
            usuarioBanco.CPF = usuario.CPF;
            usuarioBanco.NIF = usuario.NIF;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.RG = usuario.RG;
            usuarioBanco.TipoUsuarioID = usuario.TipoUsuarioID;
            usuarioBanco.EnderecoID = usuario.EnderecoID;
            usuarioBanco.CargoID = usuario.CargoID;

            _context.SaveChanges();
        }

        public void AtualizarStatus(Guid id, bool status)
        {
            Usuario usuarioBanco = _context.Usuario.Find(id);

            if (usuarioBanco == null)
                return;

            usuarioBanco.Ativo = status;

            _context.SaveChanges();
        }

        public Usuario ObterPorNIFComTipoUsuario(string nif)
        {
            return _context.Usuario
                .Include(u => u.TipoUsuario)
                .AsNoTracking()
                .FirstOrDefault(u => u.NIF == nif);
        }

        public void AtualizarSenha(Guid id, string senha)
        {
            Usuario usuarioBanco = _context.Usuario.Find(id);

            if (usuarioBanco == null)
                return;

            usuarioBanco.Senha = CriptografiaUsuario.CriptografiaSenha(senha);
            _context.SaveChanges();
        }

        public void AtualizarPrimeiroAcesso(Guid id, bool status)
        {
            Usuario usuarioBanco = _context.Usuario.Find(id);

            if (usuarioBanco == null)
                return;

            usuarioBanco.PrimeiroAcesso = status;
            _context.SaveChanges();
        }
    }
}
