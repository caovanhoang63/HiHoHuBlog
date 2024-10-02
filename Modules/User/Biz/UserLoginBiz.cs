using HiHoHuBlog.modules.user.model;
using HiHoHuBlog.Modules.User.Model;

namespace HiHoHuBlog.Modules.User.Biz;


interface IUserLoginBiz
{
    Task Login(UserLogin userLogin);
}

interface IUserLoginStore
{
    Task Login(UserLogin userLogin);
}

public class UserLoginBiz : IUserLoginBiz
{
    private readonly IUserLoginStore _userLoginStore;

    UserLoginBiz(IUserLoginStore userLoginStore)
    {
        _userLoginStore = userLoginStore;
    }

    public async Task Login(UserLogin userLogin)
    {
        await _userLoginStore.Login(userLogin);
    }
}