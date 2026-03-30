using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.BairroDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class BairroParaDto
    {
        public static ListarBairroDto ListarBairroParaDto(Bairro b)
        {
            return new ListarBairroDto
            {
                BairroID = b.BairroID,
                NomeBairro = b.NomeBairro,
                CidadeID = b.CidadeID,
            };
        }
    }
}
