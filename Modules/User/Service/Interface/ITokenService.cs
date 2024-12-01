using System.Security.Claims;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface ITokenService
{
    Task<Result<ClaimsPrincipal, Err>> ValidateToken(string token);
    Task<Result<string,Err>>  GenerateToken(string userEmail, int expireMinutes = 5);

}