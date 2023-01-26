using IdentityServices.Models.Users;

namespace IdentityServices.Contracts
{
    public interface IUsersService
    {
        Task<UserServiceModel> SignInAsync(string username, string password);

        Task<UserServiceModel?> GetUserByIdAsync(string id);

        Task<bool> RegisterAsync(UserInputServiceModel inputServiceModel);

        Task<bool> IsUsernameExistsAsync(string username);

        Task<bool> AssignRoleAsync(UserRoleInputServiceModel inputServiceModel);
    }
}
