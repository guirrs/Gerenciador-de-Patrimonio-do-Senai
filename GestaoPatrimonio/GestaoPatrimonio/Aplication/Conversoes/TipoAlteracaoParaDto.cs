using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.TipoAlteracao;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class TipoAlteracaoParaDto
    {
        public static ListarTipoAlteracao TipoAlteracaoListarParaDto(TipoAlteracao t)
        {
            return new ListarTipoAlteracao
            {
                NomeTipo = t.NomeTipo,
                TipoAlteracaoID = t.TipoAlteracaoID,
            };
        }
    }
}
