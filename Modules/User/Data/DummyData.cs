using HiHoHuBlog.Modules.User.Biz;
using HiHoHuBlog.modules.user.model;

namespace HiHoHuBlog.Modules.User.Data;

public class DummyData : IUserSignUpStore
{
    public async Task UserSignUp(UserSignUp userSignUp)
    {
        Console.WriteLine("Dummy Data");
        await Task.Delay(100);
    }
}