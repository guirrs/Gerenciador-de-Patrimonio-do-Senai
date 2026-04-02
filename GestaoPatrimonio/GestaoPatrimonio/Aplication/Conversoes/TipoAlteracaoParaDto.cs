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
        public static TipoAlteracao DtoParaDomain(CriarTipoAlteracao dto, Guid? id)
        {
            return new TipoAlteracao
            {
                TipoAlteracaoID = id ?? Guid.Empty,
                NomeTipo = dto.NomeTipo,
            };
        }
    }
}
