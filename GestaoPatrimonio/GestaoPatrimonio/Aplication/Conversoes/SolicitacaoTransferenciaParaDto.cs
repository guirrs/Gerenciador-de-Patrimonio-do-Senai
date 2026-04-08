using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.SolicitacaoTransferenciaDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class SolicitacaoTransferenciaParaDto
    {
        public static ListarSolicitacaoTransferenciaDto ConverterParaDto(SolicitacaoTransferencia solicitacao)
        {
            return new ListarSolicitacaoTransferenciaDto
            {
                DataCriacaoSolicitante = solicitacao.DataCriacaoSolicitante,
                DataResposta = solicitacao.DataResposta,
                Justificativa = solicitacao.Justificativa,
                LocalizacaoID = solicitacao.LocalizacaoID,
                NomePatrimonio = solicitacao.Patrimonio.Denominacao,
                UsuarioIDAprovacao = solicitacao.UsuarioIDAprovacao,
                PatrimonioID = solicitacao.PatrimonioID,
                StatusTransferenciaID = solicitacao.TransferenciaID,
                UsuarioIDSolicitacao = solicitacao.UsuarioIDSolicitacao,
                TransferenciaID = solicitacao.TransferenciaID,
            };
        }
    }
}
