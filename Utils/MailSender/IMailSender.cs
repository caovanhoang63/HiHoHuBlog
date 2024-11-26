namespace HiHoHuBlog.Utils.MailSender;

public interface IMailSender
{
    Task<Result<Unit,Err>> Send(MailRequest request);
}