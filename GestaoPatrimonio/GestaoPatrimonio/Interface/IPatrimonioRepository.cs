using GestaoPatrimonio.Domains;

namespace GestaoPatrimonio.Interface
{
    public interface IPatrimonioRepository
    {
        List<Patrimonio> Listar();
        Patrimonio ObterPorId(Guid id);
        Patrimonio ObterPorDenominacao(string nome);
        bool LocalizacaoExiste(Guid localizacaoId);
        bool StatusPatrimonioExiste(Guid patrimonioId);
        void Adicionar(Patrimonio patrimonio);
        void AtualizarStatus(Patrimonio patrimonio);
        void AdicionarLof(LogPatrimonio log);

        Localizacao BuscarLocalizacaoPorNome(string nome);
        StatusPatrimonio BuscarStatusPatrimonioPorNome(string nome);
        TipoAlteracao BuscarTipoAlteracaoPorNome(string nome);
    }
}
