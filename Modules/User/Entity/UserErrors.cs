using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;

public static class UserErrors 
{
    public static Err ErrUserAlreadyExists(string email)
    {
        return new Err($"{email} already exists.");
    }
    public static Err PasswordDoNotMatch()
    {
        return new Err($"Password does not match.");
    }
}