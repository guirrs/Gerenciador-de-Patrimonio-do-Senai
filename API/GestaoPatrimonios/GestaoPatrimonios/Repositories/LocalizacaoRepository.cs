using GestaoPatrimonios.Contexts;
using GestaoPatrimonios.Domains;
using GestaoPatrimonios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestaoPatrimonios.Repositories
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
            return _context.Localizacao
                .OrderBy(localizacao =>  localizacao.NomeLocal).ToList();
        }

        public Localizacao BuscarPorId(Guid localizacaoId)
        {
            return _context.Localizacao.Find(localizacaoId);
        }

        public bool UsuarioExiste(Guid usuarioId)
        {
            return _context.Usuario.Any(usuario => usuario.UsuarioID == usuarioId);
        }

        public void Adicionar(Localizacao localizacao)
        {
            _context.Localizacao.Add(localizacao);
            _context.SaveChanges();
        }

        public bool AreaExiste(Guid areaId)
        {
            return _context.Area.Any(area => area.AreaID == areaId);
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

        public void VincularUsuario(Guid localizacaoId, Guid usuarioId)
        {
            Localizacao localizacao = _context.Localizacao
                .Include(local => local.Usuario)
                .FirstOrDefault(local => local.LocalizacaoID == localizacaoId);

            Usuario usuario = _context.Usuario.Find(usuarioId);

            if (localizacao == null || usuario == null)
            {
                return;
            }

            localizacao.Usuario.Add(usuario);

            _context.SaveChanges();
        }

        public Localizacao BuscarPorNome(string nomeLocal, Guid areaId)
        {

            return _context.Localizacao.FirstOrDefault(local => local.NomeLocal.ToLower() == nomeLocal.ToLower() && local.AreaID == areaId
            );
        }
    }
}
