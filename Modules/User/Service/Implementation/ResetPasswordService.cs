using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Amazon.SimpleEmail.Model;
using HiHoHuBlog.Modules.Email.Comp.MailSender;
using HiHoHuBlog.Modules.Email.Entity;
using HiHoHuBlog.Modules.Email.Repository;
using HiHoHuBlog.Modules.Email.Service;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class ResetPasswordService(IUserRepository userRepository, IMailSender mailSender, IEmailRepository emailRepository, ITokenService tokenService) : IResetPasswordService
{
    private readonly IMailSender _mailSender = mailSender;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IEmailRepository _emailRepository = emailRepository;
    private readonly ITokenService _tokenService = tokenService;
    public async Task<Result<Unit, Err>> SendResetPassword(string email)
    {
        var old =await  _userRepository.FindByEmail(email);
        
        if (!old.IsOk) return Result<Unit, Err>.Err(old.Error);
        
        if (old.Value is null) return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("email"));

        
        var template = await _emailRepository.FindByName("RESET_PASSWORD");
        
        if (!template.IsOk || template.Value is null) return Result<Unit, Err>.Err(UtilErrors.InternalServerError(template.Error));
        
        
        
        // JWT => Secret => Hash Claims
        // TODO: Claims = [email, expired time = 5p]
        // JWT => Secret => Hash Claims
        
        // 

        var token = await _tokenService.GenerateToken(email);
        if (!token.IsOk) return Result<Unit, Err>.Err(token.Error);
        var req = new MailRequest
        {
            toAddresses = new List<string>(){email},
            bodyHtml =template.Value.Content.Replace("{token}", token.Value),
            bodyText = "",
            subject = "Reset Password",
            senderAddress = "no-reply@hihohu.site"
        };
        
        
         var r =  await _mailSender.Send(req);
         
         if (!r.IsOk) return Result<Unit, Err>.Err(r.Error);
        
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> ResetPassword(string token, string newPassword, string confirmPassword)
    {
        var tokenResult = await _tokenService.ValidateToken(token);
        if (!tokenResult.IsOk) return Result<Unit, Err>.Err(UtilErrors.InternalServerError(tokenResult.Error));

        var expirationClaim = tokenResult.Value.FindFirst("exp");
        if (expirationClaim != null && DateTimeOffset.Now.ToUnixTimeSeconds() >Int64.Parse(expirationClaim.Value) )
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrTokenExpired());
        }
        if (newPassword != confirmPassword)
        {
            return Result<Unit, Err>.Err(UserErrors.PasswordDoNotMatch());
        }
        
        var emailClaim = tokenResult.Value.FindFirst("user_email");
        if (emailClaim == null)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(new Exception("Invalid Token")));
        }
        
        var userResult = await _userRepository.FindByEmail(emailClaim.Value);
        if (!userResult.IsOk || userResult.Value == null)
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("email"));
        }
        byte[] buffer = Encoding.UTF8.GetBytes(newPassword + userResult.Value.Salt);
        var password=  Encoding.UTF8.GetString(SHA256.HashData(buffer));
        var updateResult = await _userRepository.UpdatePassword(emailClaim.Value, password);
        if (!updateResult.IsOk) return Result<Unit, Err>.Err(updateResult.Error);

        return Result<Unit, Err>.Ok(new Unit());
    }
}