using AutoMapper;
using Fitness.InputModels.Identity.Login;
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
        public async Task<IActionResult> Post(LoginInputModel inputModel)
        {
            if (await this.usersService.IsUsernameExistsAsync(inputModel.Username))
            {
                return BadRequest("An usar with that name already exists!");
            }

            UserInputServiceModel inputServiceModel = inputModel.To<UserInputServiceModel>(this.mapper);

            if (!await this.usersService.RegisterAsync(inputServiceModel))
            {
                return this.NotFound();
            }

            return this.Ok();
        }
    }
}
