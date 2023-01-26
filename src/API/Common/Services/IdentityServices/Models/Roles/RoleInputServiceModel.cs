using Fitness.Data.Models;
using MappingServices;

namespace IdentityServices.Models.Roles
{
    public class RoleInputServiceModel : IMapTo<Role>
    {
        public string? Name { get; set; }
    }
}
