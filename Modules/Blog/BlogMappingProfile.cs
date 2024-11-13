using AutoMapper;
using HiHoHuBlog.Modules.Blog.Entity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HiHoHuBlog.Modules.Blog;

public class BlogMappingProfile : Profile
{
    public BlogMappingProfile()
    {
        CreateMap<BlogCreate, Entity.Blog>();
        CreateMap<BlogMetaDataUpdate, Entity.Blog>();
        CreateMap<BlogDetail, Entity.Blog>();
        CreateMap<Entity.Blog,BlogDetail>();
        CreateMap<Entity.Blog,BlogList>();
        CreateMap<User.Entity.User, UserDto>();

    }
}