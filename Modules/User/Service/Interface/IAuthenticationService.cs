using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IAuthenticateService
{
    Task<Result<Unit, Err>> SignInUserAsync(HttpContext context, UserAuth userAuth);
}