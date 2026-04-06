using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface IUsuarioRepository
    {
        List<Usuario> Listar();
        Usuario ObterPorId(Guid id);
        Usuario BuscarDuplicado(string nif, string cpf, string email, Guid? usuarioId = null);
        bool EnderecoExiste(Guid enderecoId);
        bool CargoExiste(Guid cargoId);
        bool TipoUsuarioExiste(Guid tipoUsuarioId);
        void Adicionar(Usuario usuario);
        void Atualizar(Usuario usuario);
        void AtualizarStatus(Guid id, bool status);
        Usuario ObterPorNIFComTipoUsuario(string nif);
        void AtualizarSenha(Guid id, string senha);
        void AtualizarPrimeiroAcesso(Guid id, bool status);
        //void Remover(Usuario usuario);
    }
}
