using AutoMapper;
using HiHoHuBlog.Modules.Email.Entity;

namespace HiHoHuBlog.Modules.Email;

public class EmailMappingProfile : Profile
{
    public EmailMappingProfile()
    {
        CreateMap<EmailTemplateCreate, EmailTemplate>();
        CreateMap<EmailTemplateUpdate, EmailTemplate>();
    }
}