using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserProfileService
{
    Task<Result<UserProfile?, Err>> GetUserProfile(string userEmail);
}