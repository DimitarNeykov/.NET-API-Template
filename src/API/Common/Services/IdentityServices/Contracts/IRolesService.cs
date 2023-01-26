using IdentityServices.Models.Roles;

namespace IdentityServices.Contracts
{
    public interface IRolesService
    {
        Task<bool> AddAsync(RoleInputServiceModel inputServiceModel);
    }
}
