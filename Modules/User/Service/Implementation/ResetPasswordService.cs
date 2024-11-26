using HiHoHuBlog.Modules.Email.Comp.MailSender;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class ResetPasswordService(IUserRepository userRepository, IMailSender mailSender) : IResetPasswordService
{
    private readonly IMailSender _mailSender = mailSender;

    public async Task<Result<Unit, Err>> SendResetPassword(string email)
    {
        var old =await  userRepository.FindByEmail(email);
        
        if (!old.IsOk) return Result<Unit, Err>.Err(old.Error);
        
        if (old.Value is null) return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("email"));
        
        return Result<Unit, Err>.Ok(new Unit());
        
    }

    public Task<Result<Unit, Err>> ResetPassword(string token, string newPassword, string confirmPassword)
    {
        throw new NotImplementedException();
    }
}