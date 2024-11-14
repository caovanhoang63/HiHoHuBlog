using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class UserProfileService(IUserRepository userRepository) : IUserProfileService
{
    public async Task<Result<UserProfile?, Err>> GetUserProfile(string userEmail)
    {

        var r = await userRepository.GetProfile(userEmail);
        if (!r.IsOk)
        {
            return Result<UserProfile?, Err>.Err(r.Error);
        }

        if (r.Value is null)
        {
            return Result<UserProfile?, Err>.Err(UtilErrors.ErrEntityNotFound("user"));
        }

        r.Value.UserName = r.Value.Email.Split('@')[0];
        return Result<UserProfile?, Err>.Ok(r.Value);
    }
}