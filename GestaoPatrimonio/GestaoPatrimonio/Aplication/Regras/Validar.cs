using GestaoPatrimonio.Exceptions;

namespace GestaoPatrimonio.Aplication.Regras
{
    public class Validar
    {
        public static void ValidarNome(string nome)
        {
            if(string.IsNullOrEmpty(nome))
            {
                throw new DomainException("Nome é obrigatório");
            }
        }
    }
}
