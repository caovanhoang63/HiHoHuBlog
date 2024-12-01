using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserFollowService
{
    Task<Result<Unit,Err>> UpdateTotalFollows(int id,int userFollowId);
    
    Task<Result<Unit,Err>> Follows(int userId,int userFollowingId);
    Task<Result<Unit,Err>> UnFollow(int userId,int userFollowingId);

    
    Task<Result<bool ,Err>> IsFollowed(int userId,int userFollowingId);
}