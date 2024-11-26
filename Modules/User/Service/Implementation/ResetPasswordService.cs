using Amazon.SimpleEmail.Model;
using HiHoHuBlog.Modules.Email.Comp.MailSender;
using HiHoHuBlog.Modules.Email.Entity;
using HiHoHuBlog.Modules.Email.Repository;
using HiHoHuBlog.Modules.Email.Service;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class ResetPasswordService(IUserRepository userRepository, IMailSender mailSender, IEmailRepository emailRepository) : IResetPasswordService
{
    private readonly IMailSender _mailSender = mailSender;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailRepository _emailRepository = emailRepository;
    public async Task<Result<Unit, Err>> SendResetPassword(string email)
    {
        var old =await  _userRepository.FindByEmail(email);
        
        if (!old.IsOk) return Result<Unit, Err>.Err(old.Error);
        
        if (old.Value is null) return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("email"));

        
        var template = await _emailRepository.FindByName("RESET_PASSWORD");
        
        if (!template.IsOk || template.Value is null) return Result<Unit, Err>.Err(UtilErrors.InternalServerError(template.Error));
        

        var req = new MailRequest
        {
            toAddresses = new List<string>(){email},
            bodyHtml = template.Value.Content,
            bodyText = "",
            subject = "Reset Password",
            senderAddress = "no-reply@hihohu.site"
        };
        
         var r =  await _mailSender.Send(req);
         
         if (!r.IsOk) return Result<Unit, Err>.Err(r.Error);
        
        return Result<Unit, Err>.Ok(new Unit());
        
    }

    public Task<Result<Unit, Err>> ResetPassword(string token, string newPassword, string confirmPassword)
    {
        throw new NotImplementedException();
    }
}