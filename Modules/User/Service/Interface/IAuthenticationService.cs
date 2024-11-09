namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IAuthenticateService
{
    Task SignInUserAsync(HttpContext context, string email, string role);
}