using HiHoHuBlog.Modules.User.Biz;
using HiHoHuBlog.modules.user.model;
using HiHoHuBlog.Modules.User.Model;

namespace HiHoHuBlog.Modules.User.Data;

public class DummyData : IUserSignUpStore, IUserLoginStore
{
    public async Task UserSignUp(UserSignUp userSignUp)
    {
        Console.WriteLine("Dummy Data");
        await Task.Delay(100);
    }

    public async Task Login(UserLogin userLogin)
    {
        Console.WriteLine("Dummy Data");
        await Task.Delay(100);
    }
}