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

        public static Bairro DtoParaDomain(CriarBairroDto dto, Guid? id)
        {
            return new Bairro
            {
                BairroID = id ?? Guid.Empty,
                CidadeID = dto.CidadeID,
                NomeBairro = dto.NomeBairro,
            };
        }
    }
}
