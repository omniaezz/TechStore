using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos.UserDTO;
using TechStore.Dtos.ViewResult;
using TechStore.Models;


namespace TechStore.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly SignInManager<TechUser> _signInManager;
        private readonly UserManager<TechUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserServices(IUserRepository userRepository, IMapper mapper, SignInManager<TechUser> signInManager, UserManager<TechUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

       

        public async Task<ResultView<UpdateUserDTO>> UpdateUser(string Id ,UpdateUserDTO model)
        {
            var existUser = await _userRepository.GetByIdAsync(Id);
            if (existUser == null)
            {
                return new ResultView<UpdateUserDTO> { Entity = null, IsSuccess = false, Message = "User Not Found" };

            }
            else
            {
                if (model.UrlImage != null)
                {
                    string uploadsFolder = Path.Combine("wwwroot", "ImageUser");
                    string FileName = model.UrlImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UrlImage.CopyToAsync(fileStream);
                    }

                }
               
                existUser.UserName = model.UserName;
                existUser.Email = model.Email;
                existUser.FirstName = model.FirstName;
                existUser.LastName = model.LastName;
                existUser.Address = model.Address;
                existUser.Image = model.UrlImage.FileName;
                existUser.PhoneNumber = model.PhoneNumber;
                
                

                var result = await _userManager.UpdateAsync(existUser);
                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(existUser, isPersistent: false);
                    return new ResultView<UpdateUserDTO> { Entity = model, IsSuccess = true, Message = "User Updated Successfully" };
                }
                else
                {
                    return new ResultView<UpdateUserDTO> { Entity = model, IsSuccess = true, Message = "Error" };
                }
            }

        }





        public async Task<UpdateUserDTO> GetUserById(string ID)
        {
           var userid = await _userRepository.GetByIdAsync(ID);
            var returnuserDTO =_mapper.Map<UpdateUserDTO>(userid);
            return returnuserDTO;
        }

        public async Task<ResultDataList<UserDto>> SearchByNameUser(string Name) // add function for SearchByName
        {
            var searchedName = (await _userRepository.SearchUserByName(Name)).Select(u => new UserDto()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                PhoneNumber=u.PhoneNumber,
                Email = u.Email,
                Address = u.Address,
                Image = u.Image

            }).ToList();
            var returnuserDTO = _mapper.Map<List<UserDto>>(searchedName);

            ResultDataList<UserDto> resultDataList = new ResultDataList<UserDto>();
            resultDataList.Entities = returnuserDTO;
            resultDataList.Count = searchedName.Count();

            return resultDataList;
        }
        public async Task<ResultDataList<UserDto>> GetAllPaginationUser(int items, int pagenumber) 
        {
            var AlldAta = (await _userRepository.GetAllAsync());
            var UserSer = AlldAta.Where(u=>u.IsDeleted==false).Skip(items * (pagenumber - 1)).Take(items)
                                              .Select(u => new UserDto()
                                              {
                                                  Id=u.Id,
                                                  FirstName=u.FirstName, 
                                                  LastName=u.LastName,
                                                  UserName = u.UserName,    
                                                  Email = u.Email,
                                                  PhoneNumber=u.PhoneNumber,
                                                  Address = u.Address , 
                                                  Image=u.Image
                                                 
                                              }).ToList();
            ResultDataList<UserDto> resultDataList = new ResultDataList<UserDto>();
            resultDataList.Entities = UserSer;
            resultDataList.Count = AlldAta.Count();
            return resultDataList;
        }

        public async Task<ResultView<UserDto>> DeleteUser(string UserId)
        {
            var existUser = await _userRepository.GetByIdAsync(UserId);

            if (existUser == null)
            {
                return new ResultView<UserDto> { Entity = null, IsSuccess = false, Message = "User Not Found" };
            }
             var data= (await _userRepository.GetAllAsync()).FirstOrDefault(u=>u.Id==UserId);
             data.IsDeleted=true;
             await _userRepository.SaveChangesAsync();
            var userDeleted = _mapper.Map<UserDto>(data);
             return new ResultView<UserDto> { Entity = userDeleted, IsSuccess = true, Message = "User Deleted" };
        }

        public async Task<ResultView<RegisterDto>> RegisterUser(RegisterDto model , string RoleName="User")
        {
           
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine("wwwroot" ,"ImageUser");
                string FileName = model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }

            }
                var user = new TechUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Image = model.Image.FileName,
                    PhoneNumber = model.PhoneNumber,

                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, isPersistent: false);
                if (!await _roleManager.RoleExistsAsync(RoleName))
                {
                    // If the role does not exist, create it
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(RoleName));
                    if (!roleResult.Succeeded)
                    {
                        // Handle role creation failure
                        return new ResultView<RegisterDto> { Entity = model, IsSuccess = false, Message = "Failed to create role." };
                    }
                }

                // Add the user to the role
                await _userManager.AddToRoleAsync(user, RoleName); return new ResultView<RegisterDto> { Entity = model, IsSuccess = true, Message = "User registered successfully" };
                }
                else
                {
                    return new ResultView<RegisterDto> { Entity = model, IsSuccess = false, Message = "User registration failed" };
                }
            }


        public async Task<ResultView<LoginDto>> LoginUser(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new ResultView<LoginDto> { Entity = null, IsSuccess = false, Message = " UserName Or Password incorrcet" };
            }
            var result = _signInManager.CheckPasswordSignInAsync(user, model.Password, true).Result;
            if (!result.Succeeded)
            {
                return new ResultView<LoginDto> { Entity = null, IsSuccess = false, Message = " UserName Or Password incorrcet" };

            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return new ResultView<LoginDto> { Entity = model, IsSuccess = true, Message = " Login successfully" };
        }

        public async Task<bool> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<bool> AddRole(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            await _roleManager.CreateAsync(role);
            return true;
        }

        public async Task<List<string>> GetRoleForUser(string UserName)
        {

            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return new List<string>() {"Not Found"}; 
            }

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList(); 
        }    
        public async Task<string> GetIDForUser(string UserName)
        {

            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return "Not Found"; 
            }

            var UserId = await _userManager.GetUserIdAsync(user);

            return UserId; 
        }
        public async Task<bool> IsUserNameExists(string userName)
        {
            return await _userRepository.IsUserNameExists(userName);
        }
    }
}
