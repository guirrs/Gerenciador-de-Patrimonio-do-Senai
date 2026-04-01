using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ICargoRepository
    {
        List<Cargo> Listar();
        Cargo ObterPorId(Guid id);
        Cargo ObterPorNome(string nome);
        void Adicionar(Cargo endereco);
        void Atualizar(Cargo endereco);
    }
}
