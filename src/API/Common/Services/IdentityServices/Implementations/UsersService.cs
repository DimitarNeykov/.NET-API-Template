using IdentityServices.Contracts;
using IdentityServices.Models.Users;
using AutoMapper;
using CryptographyServices.Contracts;
using MappingServices;
using Microsoft.EntityFrameworkCore;
using Fitness.Data;
using Fitness.Data.Models;

namespace IdentityServices.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly DbRepository<User> userRepository;
        private readonly DbRepository<Role> roleRepository;
        private readonly IEncryptService encryptService;
        private readonly IMapper mapper;

        public UsersService(
            DbRepository<User> userRepository,
            DbRepository<Role> roleRepository,
            IEncryptService encryptService,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.encryptService = encryptService;
            this.mapper = mapper;
        }

        public async Task<bool> AssignRoleAsync(UserRoleInputServiceModel inputServiceModel)
        {
            User? assigner = await this.userRepository.All().FirstOrDefaultAsync(x => x.Id == inputServiceModel.AssignerId);
            Role? role = await this.roleRepository.All().FirstOrDefaultAsync(x => x.Id == inputServiceModel.RoleId);
            User? user = await this.userRepository.All().FirstOrDefaultAsync(x => x.Id == inputServiceModel.UserId);
            
            if (assigner == null || role == null || user == null
                || !assigner.Roles.Any(x => x.Name == "Administrator") && role.Name == "Administrator")
            {
                return false;
            }

            user.Roles.Add(role);
            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<UserServiceModel?> GetUserByIdAsync(string id)
        {
            User? user = await this.userRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            UserServiceModel serviceModel = user.To<UserServiceModel>(this.mapper);

            return serviceModel;
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await this.userRepository.AllAsNoTracking().AnyAsync(x => x.Username == username.ToLower());
        }

        public async Task<bool> RegisterAsync(RegisterInputServiceModel inputServiceModel)
        {
            string encryptedPassword = this.encryptService.PasswordEncrypt(inputServiceModel.Password.ToString());

            User user = inputServiceModel.To<User>(this.mapper);
            user.Username = user.Username.ToLower();
            user.Email = user.Email.ToLower();
            user.Password = encryptedPassword;

            await this.userRepository.AddAsync(user);
            await this.userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<UserServiceModel?> SignInAsync(string username, string password)
        {
            string encryptedPassword = this.encryptService.PasswordEncrypt(password);

            User? user = await this.userRepository
                .AllAsNoTracking()
                .Include(x=>x.Roles)
                .FirstOrDefaultAsync(x => x.Username == username.ToLower() && x.Password == encryptedPassword);

            if (user == null)
            {
                return null;
            }

            UserServiceModel serviceModel = user.To<UserServiceModel>(this.mapper);

            return serviceModel;
        }
    }
}
