using AutoMapper;
using Educational.Core.Common.DTO.User;
using Educational.Core.DAL.Entities;

namespace Educational.Core.BLL.MappingProfiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>();
    }
}
