using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.EnderecoDto;

namespace GestaoPatrimonio.Aplication.Conversoes
{
    public class EnderecoParaDto
    {
        public static ListarEnderecoDto EnderecoListarParaDto(Endereco dto)
        {
            return new ListarEnderecoDto
            {
                EnderecoID = dto.EnderecoID,
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                BairroID = dto.BairroID,
                NomeBairro = dto.Bairro.NomeBairro,
                CidadeId = dto.Bairro.CidadeID,
                NomeCidade = dto.Bairro.Cidade.NomeCidade,
                CEP = dto.CEP,
                Complemento = dto.Complemento,

            };
        }

        public static Endereco DtoParaDomain(CriarEnderecoDto dto, Guid? id)
        {
            return new Endereco
            {
                EnderecoID = id ?? Guid.Empty,
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                BairroID = dto.BairroID,
                CEP = dto.CEP,
                Complemento = dto.Complemento,
            };
        }
    }
}
