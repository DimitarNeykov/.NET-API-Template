using Fitness.Data.Models;
using MappingServices;

namespace IdentityServices.Models.Users
{
    public class UserServiceModel : IMapFrom<User>
    {
        public string? Id { get; set; }

        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? PhoneNumberConfirmed { get; set; }

        public ICollection<RoleServiceModel> Roles { get; set; }
    }
}
