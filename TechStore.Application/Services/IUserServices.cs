using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Dtos.AccountDtos;
using TechStore.Dtos.UserDTO;
using TechStore.Dtos.ViewResult;
using TechStore.Models;


namespace TechStore.Application.Services
{
    public interface IUserServices
    {
        Task<ResultView<UpdateUserDTO>> UpdateUser(string Id, UpdateUserDTO model );
        Task<UpdateUserDTO> GetUserById(string ID);
        Task<ResultDataList<UserDto>> GetAllPaginationUser(int items, int pagenumber);

        Task<ResultDataList<UserDto>> SearchByNameUser(string Name);//add this function 
        Task<ResultView<UserDto>> DeleteUser(string UserId);
        Task<ResultView<RegisterDto>> RegisterUser(RegisterDto model , string RoleName);
        Task<ResultView<LoginDto>> LoginUser(LoginDto model);
        Task<bool> LogoutUser();
        Task<bool> AddRole(string name); 
        Task<List<string>> GetRoleForUser(string UserName);
        Task<string> GetIDForUser(string UserName);
        Task<bool> IsUserNameExists(string userName);
    }
}
