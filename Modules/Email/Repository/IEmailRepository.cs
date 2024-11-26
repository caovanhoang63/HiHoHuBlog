using HiHoHuBlog.Modules.Email.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Email.Repository;

public interface IEmailRepository
{
    Task<Result<Unit,Err>> Create(EmailTemplateCreate c);
    Task<Result<EmailTemplate?,Err>> FindByName(string name);
    Task<Result<EmailTemplate?,Err>> FindById(int id);
    Task<Result<IEnumerable<EmailTemplate>?, Err>> FindMany(string arg, Paging paging);
    Task<Result<Unit,Err>> Update(int id ,EmailTemplateUpdate u);
    Task<Result<Unit,Err>> Delete(int id);
}