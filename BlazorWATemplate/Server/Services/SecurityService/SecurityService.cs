using System.Security.Cryptography;
using System.Text;

namespace BlazorWATemplate.Server.Services.SecurityService
{
    public class SecurityService : ISecurityService
    {
        private readonly IConfiguration _configuration;
        public SecurityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> DecryptAsync(string Input)
        {
            byte[] InputBytes = Convert.FromBase64String(Input);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = new PasswordDeriveBytes(_configuration["Security:SitePassword"], null).GetBytes(32);
            aesAlg.IV = Encoding.UTF8.GetBytes(_configuration["Security:SiteIV"]);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new(InputBytes);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return await srDecrypt.ReadToEndAsync();
        }

        public async Task<string> EncryptAsync(string Input)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = new PasswordDeriveBytes(_configuration["Security:SitePassword"], null).GetBytes(32);
                aesAlg.IV = Encoding.UTF8.GetBytes(_configuration["Security:SiteIV"]);

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    await swEncrypt.WriteAsync(Input);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
}
