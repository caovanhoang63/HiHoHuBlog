using HiHoHuBlog.Modules.User.Entity;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IAuthenticateService
{
    Task SignInUserAsync(HttpContext context,UserAuth userAuth);
}