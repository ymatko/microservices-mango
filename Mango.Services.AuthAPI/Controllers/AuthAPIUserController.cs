using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.RabbitMQSender;
using Mango.Services.AuthAPI.Service.IService;
using AutoMapper;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AuthAPIUserController : ControllerBase
    {
        private readonly IUserService _userService;
        protected ResponseDto _response;
        private readonly ILogger<AuthAPIController> _logger;
        private readonly IMapper _mapper;

        public AuthAPIUserController(IUserService userService, ILogger<AuthAPIController> logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet("GetAll")]
        public ResponseDto? GetAll()
        {
            try
            {
                var users = _userService.All();
                _response.Result = _mapper.Map<ApplicationUserDto>(users);
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
        public ResponseDto? GetUser(string userId)
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
    }
}
