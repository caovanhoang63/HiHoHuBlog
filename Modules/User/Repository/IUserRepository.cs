using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Repository;

public interface IUserRepository
{
    Task<Result<Unit,Err>> Create(UserSignUp userSignUp);
    Task<Result<Entity.User?,Err>> FindByEmail(string email);
    Task<Result<Entity.UserProfile?,Err>> GetProfile(string email);
    Task<Result<IEnumerable<Entity.User>,Err>> ListUsers(UserFilter? userFilter,Paging? paging);
    Task<Result<Entity.User?, Err>> FindByEmailAndUserName(string email, string userName);
    Task<Result<Unit,Err>> UpdateSettingsProfile(UserSettingsProfile userSettingsProfile);
    Task<Result<Entity.UserSettingsProfile?,Err>> GetSettingsProfile(string userName);
    Task<Result<IEnumerable<UserList>?,Err>> GetFollower(int userId,Paging paging);
    Task<Result<IEnumerable<UserList>?,Err>> GetFollowing(int userId,Paging paging);

    Task<Result<Unit, Err>> UpdateTotalFollows(int id,int userFollowId);
    Task<Result<Unit, Err>> Follows(int userId, int userFollowId);
    Task<Result<Unit, Err>> UnFollow(int userId,int userFollowId);
    Task<Result<bool,Err>> IsFollowed(int userId,int userFollowId);
    Task<Result<Unit, Err>> UpdatePassword(string email, string password);

    Task<Result<Entity.User?, Err>> FindById(int id);

    Task<Result<IEnumerable<UserList>?,Err>> GetUserLists(UserFilter? userFilter,Paging? paging);
    Task<Result<Unit, Err>> DeleteUser(int id);
    Task<Result<Unit, Err>> ReActiveUser(int id);
    Task<Result<Unit,Err>> UpdateRole(string email ,string role);
    Task<Result<bool[],Err>> CheckBulkFollow(int userId,List<int> userFollowId);
}
