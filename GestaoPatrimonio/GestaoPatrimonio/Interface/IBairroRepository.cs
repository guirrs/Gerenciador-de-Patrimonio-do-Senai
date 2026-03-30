using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface IBairroRepository
    {
        List<Bairro> Listar();
        Bairro BuscarPorId(Guid bairroID);
        void Adicionar (Bairro bairro);
        void Atualizar(Bairro bairro);
        Bairro BuscarPorNome(string nome, Guid cidadeID);
        bool CidadeExiste(Guid bairroID);
    }
}
