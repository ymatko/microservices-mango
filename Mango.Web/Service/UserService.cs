using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseService _baseService;
        public UserService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> GetAllAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/user/GetAll"
            });
        }

        public async Task<ResponseDto?> GetUserAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/user/GetUser/" + userId
            });
        }

        public async Task<ResponseDto?> Update(ApplicationUserDto userDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Data = userDto,
                Url = SD.AuthAPIBase + "/api/user/UpdateUser"
            });
        }
    }
}
