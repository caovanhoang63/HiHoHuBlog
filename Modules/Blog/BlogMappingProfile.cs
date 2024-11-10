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
    }
}