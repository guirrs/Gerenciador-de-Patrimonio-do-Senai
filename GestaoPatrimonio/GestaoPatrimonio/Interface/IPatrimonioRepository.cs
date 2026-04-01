using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface IPatrimonioRepository
    {
        List<Patrimonio> Listar();
        Patrimonio ObterPorId(Guid id);
        Patrimonio ObterPorDenominacao(string nome);
        void Adicionar(Patrimonio patrimonio);
        void Atualizar(Patrimonio patrimonio);
        void Remover(Guid id);
    }
}
