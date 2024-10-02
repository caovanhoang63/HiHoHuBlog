using HiHoHuBlog.Modules.User.Model;

namespace HiHoHuBlog.Modules.User.Biz;

public interface IUserLoginBiz
{
    Task Login(UserLogin userLogin);
}

public interface IUserLoginStore
{
    Task Login(UserLogin userLogin);
}

public class UserLoginBiz : IUserLoginBiz
{
    private readonly IUserLoginStore _userLoginStore;

    public UserLoginBiz(IUserLoginStore userLoginStore)
    {
        _userLoginStore = userLoginStore;
    }

    public async Task Login(UserLogin userLogin)
    {
        await _userLoginStore.Login(userLogin);
    }
}