using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserProfileService
{
    Task<Result<UserProfile?, Err>> GetUserProfile(string userName);
    Task<Result<IEnumerable<UserList>?,Err>> GetFollowers(int userId,Paging paging);
    Task<Result<IEnumerable<UserList>?,Err>> GetFollowings(int userId,Paging paging);
}