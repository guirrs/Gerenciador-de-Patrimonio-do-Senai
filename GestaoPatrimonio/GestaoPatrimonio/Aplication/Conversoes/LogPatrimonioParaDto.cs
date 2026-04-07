using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.LogPatrimonioDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class LogPatrimonioParaDto
    {
        public static ListarLogPatrimonioDto ConverterParaDto(LogPatrimonio log)
        {
            return new ListarLogPatrimonioDto
            {
                LogPatrimonioId = log.LogPatrimonioID,
                DataTransferencia = log.DataTransferencia,
                DenomoinacaoPatrimonio = log.Patrimonio.Denominacao,
                PatrimonioId = log.PatrimonioID,
                TipoAlteracao = log.TipoAlteracao.NomeTipo,
                Usuario = log.Usuario.Nome,
                Local = log.Localizacao.NomeLocal,
                StautusPatrimonio = log.StatusPatrimonio.NomeStatus
            };
        }
    }
}
