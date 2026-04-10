using GestaoPatrimonio.Domains;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonio.Interface
{
    public interface ISolicitacaoTransferenciaRepository
    {
        List<SolicitacaoTransferencia> Listar();
        SolicitacaoTransferencia ObterPorId(Guid id);
        bool ExisteSolicitacaoPendente(Guid patrimonioId);
        bool UsuarioReponsavelDaLocalizacao(Guid usuarioId, Guid localizacaoId);
        StatusTransferencia BuscarStatusTransferenciaPorNome(string nomeStatus);
        void Adicionar(SolicitacaoTransferencia solicitacao);
        bool LocalizacaoExiste(Guid localizacaoId);
        Patrimonio BuscarPatrimonioPorId(Guid patrimonioId);
        bool SolicitacaoExiste(Guid localizacaoId);
        StatusPatrimonio BuscarStatusPatrimonioPorNome(string nome);
        TipoAlteracao BuscarTipoAlteracaoPorNome(string nome);

        void Atualizar(SolicitacaoTransferencia solicitacao);
        void AtualizarPatrimonio(Patrimonio patrimonio);
        void AdicionarLog(LogPatrimonio log);
    }
}
