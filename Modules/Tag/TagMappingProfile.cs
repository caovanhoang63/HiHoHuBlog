using AutoMapper;
using HiHoHuBlog.Modules.Tag.Entity;

namespace HiHoHuBlog.Modules.Tag;

public class TagMappingProfile : Profile
{
    public TagMappingProfile()
    {
        CreateMap<TagCreate, Entity.Tag>();
    }
}