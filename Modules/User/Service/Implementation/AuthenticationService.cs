using System.Security.Claims;
using HiHoHuBlog.Modules.User.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class AuthenticateService : IAuthenticateService
{
    public async Task SignInUserAsync(HttpContext context,string id, string email, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
        };
        var identity = new ClaimsIdentity(claims, AuthConstant.Scheme);
        var principal = new ClaimsPrincipal(identity);

        await context.SignInAsync(principal);
    }
    
}