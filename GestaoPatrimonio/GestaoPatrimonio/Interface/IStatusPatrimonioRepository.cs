using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface IStatusPatrimonioRepository
    {
        List<StatusPatrimonio> Listar();
        StatusPatrimonio ObterPorId(Guid id);
        StatusPatrimonio ObterPorNome(string nome);
        void Adicionar(StatusPatrimonio tipo);
        void Atualizar(StatusPatrimonio tipo);
    }
}
