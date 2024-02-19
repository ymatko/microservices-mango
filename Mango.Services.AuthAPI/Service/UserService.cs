using AutoMapper;
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Mango.Services.AuthAPI.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;
        public UserService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ApplicationUserDto>> All()
        {
            var users = await _db.ApplicationUsers.ToListAsync();
            if(users == null)
            {
                return Enumerable.Empty<ApplicationUserDto>();
            }
            var applicationUserDto = _mapper.Map<IEnumerable<ApplicationUserDto>>(users);
            return applicationUserDto;
        }

        public async Task<ApplicationUserDto> GetById(string userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id.ToLower() == userId.ToLower());
            if (user == null)
            {
                return new ApplicationUserDto();
            }
            var applicationUserDto = _mapper.Map<ApplicationUserDto>(user);
            return applicationUserDto;
        }

        public async Task<ApplicationUserDto> Update(ApplicationUserDto userDto)
        {

            var userFormDb = _db.ApplicationUsers.First(u => u.Id == userDto.UserId);
            if( userFormDb == null)
            {
                return new ApplicationUserDto();
            }
            userFormDb.Name = userDto.Name;
            userFormDb.Email = userDto.Email;
            userFormDb.PhoneNumber = userDto.PhoneNumber;
            userFormDb.Role = userDto.Role;
            userFormDb.UserName = userDto.Email;
            userFormDb.NormalizedEmail = userDto.Email.ToUpper();
            userFormDb.NormalizedUserName = userDto.Email.ToUpper();
            _db.ApplicationUsers.Update(userFormDb);
            await _db.SaveChangesAsync();
            return _mapper.Map<ApplicationUserDto>(userFormDb);
        }
    }
}
