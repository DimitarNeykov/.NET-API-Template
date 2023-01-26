using IdentityServices.Models.Authentications;
using MappingServices;

namespace Fitness.InputModels.Identity.Login
{
    public class LoginInputModel : IMapTo<LoginInputServiceModel>
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}
