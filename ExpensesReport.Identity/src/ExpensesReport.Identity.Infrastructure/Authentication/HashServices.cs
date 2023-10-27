using System.Security.Cryptography;
using System.Text;

namespace ExpensesReport.Identity.Infrastructure.Authentication
{
    public class HashServices
    {
        public static string Encrypt(string dataToEncrypt)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(dataToEncrypt));

            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
