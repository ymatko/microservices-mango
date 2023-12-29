using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<UserDto> Register(RegisterationRequestDto registerationRequestDto);
        Task<LoginRequestDto> Login(LoginRequestDto loginRequestDto);
    }
}
