using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.API.Areas.Identity
{
    [ApiController]
    [Route("api/Identity/[controller]")]
    public class BaseIdentityController : ControllerBase
    {
        protected readonly IMapper mapper;

        public BaseIdentityController(IMapper mapper)
        {
            this.mapper = mapper;
        }
    }
}
