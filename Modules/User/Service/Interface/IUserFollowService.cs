using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserFollowService
{
    Task<Result<Unit,Err>> UpdateTotalFollows(int id);
    
    Task<Result<Unit,Err>> Follows(int userId,int userFollowingId);
}