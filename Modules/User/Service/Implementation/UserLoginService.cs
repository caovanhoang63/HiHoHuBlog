using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class UserLoginService(IUserRepository userLoginStore) : IUserLoginService
{
    public async Task Login(UserLogin userLogin)
    {
        await userLoginStore.FindByEmail(userLogin.Email);
    }
}