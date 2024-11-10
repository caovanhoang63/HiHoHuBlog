namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IAuthenticateService
{
    Task SignInUserAsync(HttpContext context,string id, string email, string role);
}