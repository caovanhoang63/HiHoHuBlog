using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserLoginService
{
   Task<Result<Unit, Err>> Login(UserLogin userLogin);
}