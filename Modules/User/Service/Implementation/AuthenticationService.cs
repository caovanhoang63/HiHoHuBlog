using System.Security.Claims;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class AuthenticateService(IUserRepository userRepository) : IAuthenticateService
{
    public async Task<Result<Unit, Err>> SignInUserAsync(HttpContext context, UserAuth u)
    {
        var vR = u.Validate();
        if (!vR.IsOk)
        {
            return Result<Unit,Err>.Err(vR.Error);
        }
        var old = await userRepository.FindByEmail(u.Email);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
            new Claim(ClaimTypes.Email, u.Email),
            new Claim(ClaimTypes.Role, u.Role),
            new Claim(ClaimTypes.Name, u.Username),
            new Claim(ClaimTypes.Uri, old.Value.Avatar.Url is not null ? old.Value.Avatar.Url : ""),
            
        };
        
        var identity = new ClaimsIdentity(claims, AuthConstant.Scheme);
        var principal = new ClaimsPrincipal(identity);
        
        
        await context.SignInAsync(principal);
        return Result<Unit, Err>.Ok(new Unit());
    }
}