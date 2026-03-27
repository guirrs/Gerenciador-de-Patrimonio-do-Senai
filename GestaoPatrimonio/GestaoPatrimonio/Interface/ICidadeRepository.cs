using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface ICidadeRepository
    {
        List<Cidade> Listar();
        Cidade ObterPorId(Guid cidadeId);
        Cidade ObterPorNome(string nome);
        Cidade ObterPorNomeEEstado(string nomeCidade, string nomeEstado);
    }
}
