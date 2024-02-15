using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Mango.Services.AuthAPI.Models.Dto;
using AutoMapper;
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class AuthAPIUserController : ControllerBase
    {
        private readonly IUserService _userService;
        protected ResponseDto _response;
        private readonly ILogger<AuthAPIController> _logger;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public AuthAPIUserController(IUserService userService, ILogger<AuthAPIController> logger, IMapper mapper, AppDbContext db)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
            _db = db;
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
                var user = _mapper.Map<ApplicationUser>(userDto);
                _db.Users.Update(user);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ApplicationUserDto>(user);
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
