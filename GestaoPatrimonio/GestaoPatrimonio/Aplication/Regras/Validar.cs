using GestaoPatrimonio.Domains;
using GestaoPatrimonio.DTOs.UsuarioDto;
using GestaoPatrimonio.Exceptions;
using GestaoPatrimonio.Interface;

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

        public static void ValidarLougadouro(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("Lougadouro é obrigatório");
            }
        }
        public static void ValidarNIF(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("NIF é obrigatório");
            }
        }
        public static void ValidarCPF(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("CPF é obrigatório");
            }
        }

        public static void ValidarEmail(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new DomainException("Email é obrigatório");
            }
        }
    }
}
