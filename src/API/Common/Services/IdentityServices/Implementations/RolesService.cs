using AutoMapper;
using Fitness.Data;
using Fitness.Data.Models;
using IdentityServices.Contracts;
using IdentityServices.Models.Roles;
using MappingServices;

namespace IdentityServices.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly DbRepository<Role> roleRepository;
        private readonly IMapper mapper;

        public RolesService(DbRepository<Role> roleRepository, IMapper mapper)
        {
            this.roleRepository = roleRepository;
            this.mapper = mapper;
        }

        public async Task<bool> AddAsync(RoleInputServiceModel inputServiceModel)
        {
            Role role = inputServiceModel.To<Role>(this.mapper);
            await this.roleRepository.AddAsync(role);
            await this.roleRepository.SaveChangesAsync();

            return true;
        }
    }
}
