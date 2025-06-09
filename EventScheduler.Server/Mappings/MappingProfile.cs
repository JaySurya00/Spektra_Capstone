using AutoMapper;
using EventScheduler.Server.Models;
using EventScheduler.Server.DTOs;


namespace EventScheduler.Server.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserSignupDto, User>();
        }
    }
}
