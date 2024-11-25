using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Repository;

public interface IUserRepository
{
    Task<Result<Unit,Err>> Create(UserSignUp userSignUp);
    Task<Result<Entity.User?,Err>> FindByEmail(string email);
    Task<Result<Entity.UserProfile?,Err>> GetProfile(string email);
    Task<Result<Entity.User?, Err>> FindByEmailAndUserName(string email, string userName);
    Task<Result<Unit, Err>> UpdateTotalFollows(int id);
    Task<Result<Unit, Err>> Follows(int userId, int userFollowId);


}