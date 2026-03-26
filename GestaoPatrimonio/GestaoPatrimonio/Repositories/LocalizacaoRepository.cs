using GestaoPatrimonio.Contexts;
using GestaoPatrimonio.Domains;
using GestaoPatrimonio.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPatrimonio.Repositories
{
    public class LocalizacaoRepository : ILocalizacaoRepository
    {
        private readonly GestaoPatrimoniosContext _context;

        public LocalizacaoRepository(GestaoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Localizacao> Listar()
        {
            return _context.Localizacao.OrderBy(l => l.NomeLocal).ToList();
        }

        public Localizacao ObterPorID(Guid id)
        {
            return _context.Localizacao.Find(id);
        }

        public bool NomeExiste(string nome)
        {
            return _context.Localizacao.Any(l => l.NomeLocal == nome);
        }

        public bool AreaExiste(Guid areaId)
        {
            return _context.Area.Any(a => a.AreaID == areaId);
        }

        public void Adicionar(Localizacao localizacao)
        {
            _context.Localizacao.Add(localizacao);
            _context.SaveChanges();
        }

        public void Atualizar(Localizacao localizacao)
        {
            if(localizacao == null)
            {
                return;
            }

            Localizacao localizacaoBanco = _context.Localizacao.Find(localizacao.LocalizacaoID);

            if(localizacaoBanco == null)
            {
                return;
            }

            localizacaoBanco.NomeLocal = localizacao.NomeLocal;
            localizacaoBanco.LocalSAP = localizacao.LocalSAP;
            localizacaoBanco.DescricaoSAP = localizacao.DescricaoSAP;
            localizacaoBanco.AreaID = localizacao.AreaID;

            _context.SaveChanges();
        }
    }
}
