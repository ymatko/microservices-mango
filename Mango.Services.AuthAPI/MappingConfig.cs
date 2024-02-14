using AutoMapper;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ApplicationUser, ApplicationUserDto>()
                .ForMember(dest => dest.UserId, u => u.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, u => u.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, u => u.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, u => u.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Role, u => u.MapFrom(src => src.Role))
                .ReverseMap();
            });
            return mappingConfig;
        }
    }
}
