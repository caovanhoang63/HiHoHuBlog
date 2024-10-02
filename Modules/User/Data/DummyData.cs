using HiHoHuBlog.Modules.User.Biz;
using HiHoHuBlog.modules.user.model;

namespace HiHoHuBlog.Modules.User.Data;

public class DummyData :IUseSignUpStore
{
    public void UseSignStore(UserSignUp userSignUp)
    {
        Console.WriteLine("Dummy Data");
    }
}