using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.AreaDto;

namespace GestaoPatrimonio.Interface
{
    public interface IAreaRepository
    {
        public List<Area> Listar();
        public Area BuscarPorId(Guid id);
        public Area BuscarPorNome(string nomeArea);
        void Adicionar(Area area);
        void Atualizar(Area dto);
    }
}
