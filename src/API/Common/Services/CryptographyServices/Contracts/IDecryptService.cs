namespace CryptographyServices.Contracts
{
    public interface IDecryptService
    {
        string PasswordDecrypt(string encryptedPassword);

        string ClaimDecrypt(string encryptedClaim);
    }
}
