using Fitness.Data.Models;
using MappingServices;

namespace IdentityServices.Models.Users
{
    public class RoleServiceModel : IMapFrom<Role>
    {
        public string Name { get; set; }
    }
}
