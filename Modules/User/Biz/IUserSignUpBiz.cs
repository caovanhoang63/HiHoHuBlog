using HiHoHuBlog.modules.user.model;

namespace HiHoHuBlog.modules.user.biz;

public interface IUserSignUpBiz
{
    Task SignUp(UserSignUp userSignUp);
}