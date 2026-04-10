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

        public static void ValidarLougadouro(string lougadouro)
        {
            if (string.IsNullOrEmpty(lougadouro))
            {
                throw new DomainException("Lougadouro é obrigatório");
            }
        }
        public static void ValidarNIF(string nif)
        {
            if (string.IsNullOrEmpty(nif))
            {
                throw new DomainException("NIF é obrigatório");
            }
        }
        public static void ValidarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                throw new DomainException("CPF é obrigatório");
            }
        }

        public static void ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new DomainException("Email é obrigatório");
            }
        }

        public static void ValidarSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                throw new DomainException("Email é obrigatório");
            }
        }

        public static void ValidarJustificativa(string justificativa)
        {
            if (string.IsNullOrEmpty(justificativa))
            {
                throw new DomainException("Email é obrigatório");
            }
        }

        public static void ValidarNumeroPatrimonio(string numeroPatrimonio)
        {
            if (string.IsNullOrEmpty(numeroPatrimonio))
            {
                throw new DomainException("Email é obrigatório");
            }
        }
    }
}
