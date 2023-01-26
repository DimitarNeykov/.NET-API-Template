using IdentityServices.Models.Authentications;
using MappingServices;

namespace Fitness.ResponseModels.Identity.Login
{
    public class TokenResponseModel : IMapFrom<TokenServiceModel>
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
