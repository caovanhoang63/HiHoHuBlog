using HiHoHuBlog.modules.user.model;

namespace HiHoHuBlog.Modules.User.Biz;

public interface IUserSignUpBiz
{
    Task SignUp(UserSignUp userSignUp);
}

public interface IUserSignUpStore
{
    Task UserSignUp(UserSignUp userSignUp);
}

public class UserSignUpBiz : IUserSignUpBiz
{
    private readonly IUserSignUpStore _userSignUpStore;

    public UserSignUpBiz(IUserSignUpStore userSignUpStore)
    {
        _userSignUpStore = userSignUpStore;
    }

    public async Task SignUp(UserSignUp userSignUp)
    {
        if (userSignUp.Password != userSignUp.ConfirmPassword)
            throw new Exception("Passwords don't match");
        Console.WriteLine("Ok1");
        await _userSignUpStore.UserSignUp(userSignUp);
    }
}