using System.Security.Claims;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class AuthenticateService : IAuthenticateService
{
    public async Task SignInUserAsync(HttpContext context,UserAuth u)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
            new Claim(ClaimTypes.Email, u.Email),
            new Claim(ClaimTypes.Role, u.Role),
        };
        
        var identity = new ClaimsIdentity(claims, AuthConstant.Scheme);
        var principal = new ClaimsPrincipal(identity);

        await context.SignInAsync(principal);
    }
    
}