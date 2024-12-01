using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;
using Nest;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUsersService
{
    Task<Result<IEnumerable<UserList>?, Err>> GetUsers(UserFilter? filter, Paging paging);
    Task<Result<Unit, Err>> DeleteUser(IRequester requester,int id);
    Task<Result<Unit, Err>> ReActiveUser(IRequester requester,int id);
    Task<Result<Unit, Err>> UpdateRoleUser(IRequester requester,string email, string role);
}