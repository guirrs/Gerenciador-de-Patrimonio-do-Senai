using GestaoPatrimonio.Domains;
using System.Xml.Serialization;

namespace GestaoPatrimonio.Interface
{
    public interface IStatusTransferenciaRepository
    {
        List<StatusTransferencia> Listar();
        StatusTransferencia ObterPorId(Guid id);
        StatusTransferencia ObterPorNome(string nome);
        void Adicionar(StatusTransferencia status);
        void Atualizar(StatusTransferencia status);
    }
}
