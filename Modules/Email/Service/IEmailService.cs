using HiHoHuBlog.Modules.Email.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Email.Service;

public interface IEmailService
{
    Task<Result<Unit,Err>> Create(IRequester requester,EmailTemplateCreate c);
    Task<Result<EmailTemplate?,Err>> FindByName(IRequester requester, string name);
    Task<Result<IEnumerable<EmailTemplate>?, Err>> FindMany(IRequester requester, string arg, Paging paging);
    Task<Result<Unit,Err>> Update(IRequester requester, string name ,EmailTemplateUpdate u);
    Task<Result<Unit,Err>> Delete(IRequester requester ,int id);
}