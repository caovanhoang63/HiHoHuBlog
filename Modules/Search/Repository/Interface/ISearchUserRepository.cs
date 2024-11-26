using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Repository.Interface;

public interface ISearchUserRepository
{
    Task<Result<IEnumerable<UserSearchDoc>?,Err>> SearchUsers(string agrs, Paging paging);
}