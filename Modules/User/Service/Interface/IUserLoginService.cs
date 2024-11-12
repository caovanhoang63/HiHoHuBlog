using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserLoginService
{
   Task<Result<UserAuth, Err>> Login(UserLogin userLogin);
}