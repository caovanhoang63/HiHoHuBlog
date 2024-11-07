using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserSignUpService
{
    Task<Result<Unit, Err>>SignUp(UserSignUp u);
}