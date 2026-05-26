using GestaoPatrimonios.Domains;

namespace GestaoPatrimonios.Interfaces
{
    public interface IBairroRepository
    {
        List<Bairro> Listar();
        Bairro BuscarPorId(Guid bairroId);
        Bairro BuscarPorNome(string nomeBairro, Guid cidadeId);
        bool CidadeExiste(Guid cidadeId);
        void Adicionar(Bairro bairro);
        void Atualizar(Bairro bairro);
    }
}