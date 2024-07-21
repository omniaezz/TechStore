using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechStore.Application.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechStore.ViewAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllAsync()
        {
            var data= await _userServices.GetAllPaginationUser(4 ,1);
            return Ok(data);

        }
        [HttpGet("SearchUsersByName")]
        public async Task<IActionResult> GetAsync(string Name)
        {
            var data= await _userServices.SearchByNameUser(Name);
            return Ok(data);

        }
        [HttpGet("GetOneUser")]
        public async Task<IActionResult> GetOneAsync(string Id)
        {
            var data= await _userServices.GetUserById(Id);
            return Ok(data);

        }
    }
}
