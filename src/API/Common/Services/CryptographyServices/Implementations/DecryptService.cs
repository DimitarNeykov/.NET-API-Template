using CryptographyServices.Contracts;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyServices.Implementations
{
    public class DecryptService : IDecryptService
    {
        private readonly IConfiguration configuration;

        public DecryptService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string ClaimDecrypt(string encryptedClaim)
        {
            string result = this.Encrypt(encryptedClaim, this.configuration["EncryptionKeys:Claim"]);

            return result;
        }

        public string PasswordDecrypt(string encryptedPassword)
        {
            string result = this.Encrypt(encryptedPassword, this.configuration["EncryptionKeys:Password"]);

            return result;
        }

        private string Encrypt(string input, string encryptionKeys)
        {
            byte[] cipherBytes = Convert.FromBase64String(input);
            string result = string.Empty;
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKeys, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    result = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return result;
        }
    }
}
