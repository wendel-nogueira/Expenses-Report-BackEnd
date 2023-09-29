using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ExpensesReport.Identity.Infrastructure.Authentication
{
    public class HashServices
    {
        public static string Encrypt(string dataToEncrypt, IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Hash:Key").Value!);
            var iv = Encoding.UTF8.GetBytes(config.GetSection("Hash:IV").Value!);

            using var aes = Aes.Create();
            using var encryptor = aes.CreateEncryptor(key, iv);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

            var data = Encoding.UTF8.GetBytes(dataToEncrypt);

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string hashToDecrypt, IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Hash:Key").Value!);
            var iv = Encoding.UTF8.GetBytes(config.GetSection("Hash:IV").Value!);

            using var aes = Aes.Create();
            using var decryptor = aes.CreateDecryptor(key, iv);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);

            var data = Convert.FromBase64String(hashToDecrypt);

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
