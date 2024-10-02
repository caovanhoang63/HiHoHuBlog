using HiHoHuBlog.modules.user.biz;
using HiHoHuBlog.modules.user.model;

namespace HiHoHuBlog.Modules.User.Biz;

public interface IUseSignUpStore
{
    void UseSignStore(UserSignUp userSignUp);
}

public class UseSignUpBiz : IUseSignUpBiz
{
    private readonly IUseSignUpStore _upStore;

    public UseSignUpBiz(IUseSignUpStore upStore)
    {
        _upStore = upStore;
    }

    public void  SignUp(UserSignUp userSignUp)
    {
        if (userSignUp.Password != userSignUp.ConfirmPassword) 
            throw new Exception("Passwords don't match");
        _upStore.UseSignStore(userSignUp);
    }
}