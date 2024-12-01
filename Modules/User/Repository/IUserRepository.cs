using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;
using Nest;

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
    Task<Result<IEnumerable<UserList>?,Err>> GetUserLists(UserFilter? userFilter,Paging? paging);
    Task<Result<Unit, Err>> DeleteUser(int id);
    Task<Result<Unit, Err>> ReActiveUser(int id);
    Task<Result<Unit,Err>> UpdateRole(string email ,string role);

}
