using HiHoHuBlog.Modules.User.Entity;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserLoginService
{
   Task Login(UserLogin userLogin);
}