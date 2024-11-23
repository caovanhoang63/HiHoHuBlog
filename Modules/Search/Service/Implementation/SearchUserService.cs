using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Modules.Search.Service.Interface;
using HiHoHuBlog.Utils;
using Quartz.Util;

namespace HiHoHuBlog.Modules.Search.Service.Implementation;

public class SearchUserService(ISearchUserRepository repo) : ISearchUserService
{
    public async Task<Result<IEnumerable<UserSearchDoc>?, Err>> SearchUsers(string arg, Paging paging)
    {
        if (arg.IsNullOrWhiteSpace())
        {
            return Result<IEnumerable<UserSearchDoc>?, Err>.Ok(null);
        }
        
        var r = await repo.SearchUsers(arg, paging);

        if (!r.IsOk)
        {
            return Result<IEnumerable<UserSearchDoc>?, Err>.Err(r.Error);
        }

        return r;
    }
}