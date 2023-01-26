using CryptographyServices.Contracts;
using IdentityServices.Contracts;
using IdentityServices.Models.Authentications;
using IdentityServices.Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServices.Implementations
{
    public class AuthenticationsService : IAuthenticationsService
    {
        private readonly IConfiguration configuration;
        private readonly IUsersService userService;
        private readonly IEncryptService encryptService;

        public AuthenticationsService(IConfiguration configuration, IUsersService userService, IEncryptService encryptService)
        {
            this.configuration = configuration;
            this.userService = userService;
            this.encryptService = encryptService;
        }

        public async Task<TokenServiceModel?> AuthenticateAsync(LoginInputServiceModel inputServiceModel)
        {
            UserServiceModel user = await this.userService.SignInAsync(inputServiceModel.Username, inputServiceModel.Password);
            if (user == null)
            {
                return null;
            }
            
            ICollection<Claim> claims = new List<Claim>();
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            claims.Add(new Claim(ClaimTypes.NameIdentifier,this.encryptService.ClaimEncrypt(user.Id)));
            claims.Add(new Claim(ClaimTypes.Email, this.encryptService.ClaimEncrypt(user.Email)));
            claims.Add(new Claim(ClaimTypes.Name, this.encryptService.ClaimEncrypt(user.FirstName)));
            claims.Add(new Claim(ClaimTypes.Surname, this.encryptService.ClaimEncrypt(user.LastName)));
            claims.Add(new Claim(ClaimTypes.MobilePhone, this.encryptService.ClaimEncrypt(user.PhoneNumber)));
            claims.Add(new Claim(ClaimTypes.UserData, this.encryptService.ClaimEncrypt(user.Username)));

            var tokenkey = Encoding.UTF8.GetBytes(this.configuration["JwtTokenValidation:Secret"]);
            
            var tokendesc = new SecurityTokenDescriptor
            {
                Audience = this.configuration["JwtTokenValidation:Audience"],
                Issuer = this.configuration["JwtTokenValidation:Issuer"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokendesc);
            string finaltoken = tokenhandler.WriteToken(token);

            TokenServiceModel tokenServiceModel = new()
            {
                Token = finaltoken,
                RefreshToken = "",
            };

            return tokenServiceModel;
        }
    }
}
