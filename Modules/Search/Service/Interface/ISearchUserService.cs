using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface ISearchUserService
{
    Task<Result<IEnumerable<UserSearchDoc>?, Err>> SearchUsers(IRequester? requester,string arg, Paging paging);
    Task<Result<IEnumerable<UserSearchDoc>?,Err>> RandomUsers(IRequester? requester, int seed ,Paging paging);
    Task<Result<IEnumerable<UserSearchDoc>?,Err>> RecommendUsers(IRequester? requester, Paging paging);
}