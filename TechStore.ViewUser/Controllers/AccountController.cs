using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechStore.Application.Services;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos.UserDTO;

namespace TechStore.ViewUser.Controllers
{
    public class AccountController :Controller
    {
        private readonly IUserServices _userServices;

        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDto register, string RoleName="User")
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (await _userServices.IsUserNameExists(register.UserName))
            {
               
                ModelState.AddModelError(nameof(register.UserName), "Username already exists.");
                return View("Register", register);
            }
            var result= await _userServices.RegisterUser(register , RoleName);
            if (result.IsSuccess)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View( "Register", register);

            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", login);
            }

            var result = await _userServices.LoginUser(login);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // If the login fails, add a model error for the UserName property
                ModelState.AddModelError(nameof(login.UserName), "Invalid username or password.");
                return View("Login", login);
            }
        }

        [Authorize]

        [HttpGet]
        public async Task<IActionResult> UpdateAccount(string Id )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user =await _userServices.GetUserById(userId);
            return View(user) ;
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateAccountAsync(string Id, UpdateUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateAccount" , model);
            }
            var data = await _userServices.UpdateUser(Id,model);   
            return View(model);
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await _userServices.LogoutUser();
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Accounthome()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userServices.GetUserById(userId);
            
            return View(user);
        }
        public IActionResult Order()
        {
            return View() ;
        }
    }
}
