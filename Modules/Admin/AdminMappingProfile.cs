using AutoMapper;
using HiHoHuBlog.Modules.Admin.Entity;

namespace HiHoHuBlog.Modules.Admin;

public class AdminMappingProfile : Profile
{
    public AdminMappingProfile()
    {
        CreateMap<BlogBlockedCreate,BlogBlocked>();
        CreateMap<ReasonBlogBlockCreate, ReasonBlogBlock>();
    }
}