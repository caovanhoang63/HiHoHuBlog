using HiHoHuBlog.Modules.Email.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Email.Comp.MailSender;

public interface IMailSender
{
    Task<Result<Unit,Err>> Send(MailRequest request);
}