using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ISolicitacaoTransferenciaRepository
    {
        List<SolicitacaoTransferencia> Listar();
        SolicitacaoTransferencia ObterPorId(Guid id);
    }
}
