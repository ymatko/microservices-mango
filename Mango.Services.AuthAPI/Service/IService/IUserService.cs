using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUserDto>> All();
        Task<ApplicationUserDto> GetById(string userId);
    }
}
