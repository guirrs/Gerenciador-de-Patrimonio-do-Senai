using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface IEnderecoRepository
    {
        List<Endereco> Listar();
        Endereco ObterPorId(Guid id);
        void Adicionar(Endereco endereco);
        void Atualizar(Endereco endereco);
        Endereco BuscarPorLougadouroENumero(string lougradoura, int? numero, Guid bairroId);
        bool BairroExiste(Guid bairroId);
    }
}
