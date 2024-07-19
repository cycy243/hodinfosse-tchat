using AutoMapper;
using Tchat.Api.Domain;
using Tchat.Api.Models;

namespace Tchat.Api.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(opt => opt.UserName))
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(opt => opt.Birthdate == null ? "" : opt.Birthdate.Value.ToString("dd/MM/yyyy")));
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(opt => opt.UserName))
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(opt => DateOnly.Parse(opt.Birthdate)));
        }
    }
}
