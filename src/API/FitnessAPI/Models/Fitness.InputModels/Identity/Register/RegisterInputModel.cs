using IdentityServices.Models.Users;
using MappingServices;

namespace Fitness.InputModels.Identity.Register
{
    public class RegisterInputModel : IMapTo<RegisterInputServiceModel>
    {
        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
