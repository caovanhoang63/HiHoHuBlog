using AutoMapper;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Search.Entity;

namespace HiHoHuBlog.Modules.Search;

public class SearchMappingProfile : Profile
{
    public SearchMappingProfile()
    {
        CreateMap<BlogList, BlogSearchDoc>();
        CreateMap<Blog.Entity.Blog, BlogSearchDoc>();
    }
}