using AutoMapper;
using PWA_API.Application.DTOs.Users;
using PWA_API.Domain.Entities;

namespace PWA_API.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()));
    }
}
