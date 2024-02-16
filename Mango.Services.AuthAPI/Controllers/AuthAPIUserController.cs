using AutoMapper;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    //[Authorize(Roles = "ADMIN")]
    public class AuthAPIUserController : ControllerBase
    {
        private readonly IUserService _userService;
        protected ResponseDto _response;
        private readonly ILogger<AuthAPIController> _logger;

        public AuthAPIUserController(IUserService userService, ILogger<AuthAPIController> logger)
        {
            _userService = userService;
            _logger = logger;
            _response = new();
        }
        [HttpGet("GetAll")]
        public ResponseDto GetAll()
        {
            try
            {
                IEnumerable<ApplicationUserDto> userDto;
                userDto = _userService.All().GetAwaiter().GetResult();
                _response.Result = userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAll users" + ex.Message);
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet("GetUser/{userId}")]
        public ResponseDto GetUser(string userId)
        {
            try
            {
                var user = _userService.GetById(userId);
                _response.Result = user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetAll users" + ex.Message);
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpPut("UpdateUser")]
        [Tags("Updaters")]
        public ResponseDto UpdateUser([FromBody] ApplicationUserDto userDto)
        {
            try
            {
                var user = _userService.Update(userDto);
                _response.Result = user;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in UpdateUser users" + ex.Message);
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
