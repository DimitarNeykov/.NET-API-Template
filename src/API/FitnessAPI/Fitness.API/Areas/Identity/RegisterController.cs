using AutoMapper;
using Fitness.InputModels.Identity.Login;
using Fitness.InputModels.Identity.Register;
using Fitness.ResponseModels.Identity.Login;
using IdentityServices.Contracts;
using IdentityServices.Models.Authentications;
using IdentityServices.Models.Users;
using MappingServices;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.API.Areas.Identity
{
    public class RegisterController : BaseIdentityController
    {
        private readonly IUsersService usersService;

        public RegisterController(IMapper mapper, IUsersService usersService)
            : base(mapper)
        {
            this.usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(RegisterInputModel inputModel)
        {
            if (await this.usersService.IsUsernameExistsAsync(inputModel.Username))
            {
                return BadRequest();
            }

            RegisterInputServiceModel inputServiceModel = inputModel.To<RegisterInputServiceModel>(this.mapper);

            if (!await this.usersService.RegisterAsync(inputServiceModel))
            {
                return this.NotFound();
            }

            return this.Created(string.Empty,null);
        }
    }
}
