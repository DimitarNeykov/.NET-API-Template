using IdentityServices.Models.Authentications;

namespace IdentityServices.Contracts
{
    public interface IAuthenticationsService
    {
        Task<TokenServiceModel?> AuthenticateAsync(LoginInputServiceModel inputServiceModel);
    }
}
