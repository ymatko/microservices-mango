using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IUserService
    {
        Task<ResponseDto?> GetAllAsync();
        Task<ResponseDto?> GetUserAsync(string userId);
        Task<ResponseDto?> Update(ApplicationUserDto userDto);

    }
}
