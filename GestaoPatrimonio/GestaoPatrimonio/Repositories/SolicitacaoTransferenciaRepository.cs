using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.Identity.Client;

namespace GestaoPatrimonio.Repositories
{
    public class SolicitacaoTransferenciaRepository : ISolicitacaoTransferenciaRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public SolicitacaoTransferenciaRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<SolicitacaoTransferencia> Listar()
        {
            return _context.SolicitacaoTransferencia.OrderByDescending(s => s.DataCriacaoSolicitante).ToList();
        }

        public SolicitacaoTransferencia ObterPorId(Guid id)
        {
            return _context.SolicitacaoTransferencia.Find(id);
        }

        public StatusTransferencia BuscarStatusTransferenciaPorNome(string nomeStatus)
        {
            return _context.StatusTransferencia.FirstOrDefault(s => s.NomeStatus.ToLower() == nomeStatus.ToLower());
        }

        public bool ExisteSolicitacaoPendente(Guid patrimonioId)
        {
            StatusTransferencia statusPendente = BuscarStatusTransferenciaPorNome("Pendente de aprovação");

            if (statusPendente == null)
                return false;

            return _context.SolicitacaoTransferencia.Any(s => 
            s.PatrimonioID == patrimonioId && 
            s.StatusTransferenciaID == statusPendente.StatusTransferenciaID);
        }

        public bool UsuarioReponsavelDaLocalizacao(Guid usuarioId, Guid localizacaoId)
        {
            return _context.Usuario.Any(u => u.UsuarioID == usuarioId &&
            u.Localizacao.Any(l => l.LocalizacaoID == localizacaoId));
        }

        public void Adicionar(SolicitacaoTransferencia solicitacao)
        {
            _context.SolicitacaoTransferencia.Add(solicitacao);
            _context.SaveChanges();
        }

        public bool SolicitacaoExiste(Guid localizacaoId)
        {
            return _context.Localizacao.Any(l => l.LocalizacaoID == localizacaoId);
        }

        public Patrimonio BuscarPatrimonioPorId(Guid id)
        {
            return _context.Patrimonio.Find(id);
        }

        public StatusPatrimonio BuscarStatusPatrimonioPorNome(string nome)
        {
            return _context.StatusPatrimonio.FirstOrDefault(status => status.NomeStatus.ToLower() == nome.ToLower());
        }

        public TipoAlteracao BuscarTipoAlteracaoPorNome(string nome)
        {
            return _context.TipoAlteracao.FirstOrDefault(tipo => tipo.NomeTipo.ToLower() == nome.ToLower());
        }

        public void Atualizar(SolicitacaoTransferencia solicitacao)
        {
            if (solicitacao == null)
                return;

            SolicitacaoTransferencia solicitacaoBanco = _context.SolicitacaoTransferencia.Find(solicitacao.TransferenciaID);

            if (solicitacaoBanco == null)
                return;

            solicitacaoBanco.DataResposta = solicitacao.DataResposta;
            solicitacaoBanco.StatusTransferenciaID = solicitacao.StatusTransferenciaID;
            solicitacaoBanco.UsuarioIDAprovacao = solicitacao.UsuarioIDAprovacao;

            _context.SaveChanges(); 
        }

        public void AtualizarPatrimonio(Patrimonio patrimonio)
        {
            if(patrimonio == null)
                return;

            Patrimonio patriomonioBanco = _context.Patrimonio.Find(patrimonio.PatrimonioID);

            if(patriomonioBanco == null)
                return;

            patriomonioBanco.LocalizacaoID = patrimonio.LocalizacaoID;
            patriomonioBanco.StatusPatrimonioID = patrimonio.StatusPatrimonioID;

            _context.SaveChanges();
        }

        public void AdicionarLog(LogPatrimonio log)
        {
            _context.LogPatrimonio.Add(log);    
            _context.SaveChanges();
        }
    }
}
