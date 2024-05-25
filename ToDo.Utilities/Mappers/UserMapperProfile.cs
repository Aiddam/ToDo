
using AutoMapper;
using ToDo.Models.Models;

namespace ToDo.Utilities.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
