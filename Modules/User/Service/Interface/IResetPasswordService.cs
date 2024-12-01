using HiHoHuBlog.Utils;
using Microsoft.AspNetCore.Identity.Data;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IResetPasswordService
{
    Task<Result<Unit,Err>> SendResetPassword(string email);
    Task<Result<Unit, Err>> ResetPassword(string token,string newPassword, string confirmPassword);
}