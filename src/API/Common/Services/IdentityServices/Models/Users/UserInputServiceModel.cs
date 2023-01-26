using Fitness.Data.Models;
using MappingServices;

namespace IdentityServices.Models.Users
{
    public class UserInputServiceModel : IMapTo<User>
    {
        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
