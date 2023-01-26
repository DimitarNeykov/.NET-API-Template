using CryptographyServices.Contracts;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyServices.Implementations
{
    public class EncryptService : IEncryptService
    {
        private readonly IConfiguration configuration;

        public EncryptService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string ClaimEncrypt(string claim)
        {
            string result = this.Encrypt(claim, this.configuration["EncryptionKeys:Claim"]);

            return result;
        }

        public string PasswordEncrypt(string password)
        {
            string result = this.Encrypt(password, this.configuration["EncryptionKeys:Password"]);

            return result;
        }

        private string Encrypt(string input, string encryptionKeys)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(input);
            string result = string.Empty;
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKeys, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    result = Convert.ToBase64String(ms.ToArray());
                }
            }

            return result;
        }
    }
}
