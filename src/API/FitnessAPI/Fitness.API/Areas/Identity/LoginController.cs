using AutoMapper;
using Fitness.InputModels.Identity.Login;
using Fitness.ResponseModels.Identity.Login;
using IdentityServices.Contracts;
using IdentityServices.Models.Authentications;
using MappingServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.API.Areas.Identity
{
    public class LoginController : BaseIdentityController
    {
        private readonly IAuthenticationsService authenticationsService;

        public LoginController(IMapper mapper, IAuthenticationsService authenticationsService)
            : base(mapper)
        {
            this.authenticationsService = authenticationsService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginInputModel inputModel)
        {
            LoginInputServiceModel inputServiceModel = inputModel.To<LoginInputServiceModel>(this.mapper);

            TokenServiceModel? serviceModel = await this.authenticationsService.AuthenticateAsync(inputServiceModel);

            if (serviceModel == null)
            {
                return this.NotFound();
            }

            TokenResponseModel responseModel = serviceModel.To<TokenResponseModel>(this.mapper);

            return this.Ok(responseModel);
        }
    }
}
