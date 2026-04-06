using System.Security.Cryptography;
using System.Text;

namespace GestaoPatrimonio.Aplication.Autenticacao
{
    public class CriptografiaUsuario
    {
        public static byte[] CriptografiaSenha(string senha)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] bytesSenha = Encoding.UTF8.GetBytes(senha);
            byte[] senhaCriptografada = sHA256.ComputeHash(bytesSenha);

            return senhaCriptografada;
        }
    }
}
