using AutoMapper;
using HiHoHuBlog.Modules.User.Entity;

namespace HiHoHuBlog.Modules.User;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserLogin, Entity.User>();
        CreateMap<UserSignUp, Entity.User>();
        CreateMap< Entity.User,UserAuth>();
    }
}