namespace CryptographyServices.Contracts
{
    public interface IEncryptService
    {
        string PasswordEncrypt(string password);

        string ClaimEncrypt(string claim);
    }
}
