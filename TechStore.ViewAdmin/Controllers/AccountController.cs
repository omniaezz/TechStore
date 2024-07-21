using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechStore.Application.Services;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos.UserDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IConfiguration config;

        public AccountController(IUserServices userServices , IConfiguration configuration)
        {
            _userServices = userServices;
            config = configuration;
        }
        [Authorize(Roles ="Admin")]
        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(RegisterDto model , string RoleName)
        {
            var data = await _userServices.RegisterUser(model,RoleName);
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(string UserId)
        {
            var data = await _userServices.DeleteUser(UserId);
            return Ok(data);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login( LoginDto model )
        {
            var data = await _userServices.LoginUser(model);
            if (data.IsSuccess)
            {
                var claims = new List<Claim>();
                var UserId = await _userServices.GetIDForUser(model.UserName);
                var role = await _userServices.GetRoleForUser(model.UserName);
                claims.Add(new Claim(ClaimTypes.Name , model.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier , UserId));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                foreach (var itemRole in role)
                {
                    
                    claims.Add(new Claim(ClaimTypes.Role, itemRole));

                }
                SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));    
                SigningCredentials  credentials =new SigningCredentials( key, SecurityAlgorithms.HmacSha256);
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: config["JWT:ValidIssure"], //api
                    audience: config["JWT:ValidAudience"],//consumer(angular)
                    claims:claims , 
                    expires: DateTime.Now.AddDays(3), 
                    signingCredentials:credentials
                    
                    );


                return Ok(new
                {
                    Token =new JwtSecurityTokenHandler().WriteToken(token), 
                    Expiration= token.ValidTo

                });

            }
            return Unauthorized();
           
        }
    }
}
