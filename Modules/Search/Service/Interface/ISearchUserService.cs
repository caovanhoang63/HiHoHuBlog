using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface ISearchUserService
{
    Task<Result<IEnumerable<UserSearchDoc>?, Err>> SearchUsers(string arg, Paging paging);
}