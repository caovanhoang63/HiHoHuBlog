using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Repository;

public interface IUserRepository
{
    Task<Result<Unit,Err>> Create(UserSignUp userSignUp);
    Task<Result<Entity.User?,Err>> FindByEmail(string email);
}