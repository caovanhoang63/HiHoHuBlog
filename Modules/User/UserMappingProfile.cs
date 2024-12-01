using AutoMapper;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.User.Entity;

namespace HiHoHuBlog.Modules.User;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<UserLogin, Entity.User>();
        CreateMap<UserSignUp, Entity.User>();
        CreateMap< Entity.User,UserAuth>();
        CreateMap<Entity.User, UserProfile>();
        CreateMap<Entity.User, UserList>();
    }
}