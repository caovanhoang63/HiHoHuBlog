using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Modules.Search.Service.Interface;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Utils;
using Nest;
using Quartz.Util;

namespace HiHoHuBlog.Modules.Search.Service.Implementation;

public class SearchUserService(ISearchUserRepository searchRepo,IUserRepository userRepo) : ISearchUserService
{
    public async Task<Result<IEnumerable<UserSearchDoc>?, Err>> SearchUsers(IRequester? requester,string arg, Paging paging)
    {
        if (arg.IsNullOrWhiteSpace())
        {
            return Result<IEnumerable<UserSearchDoc>?, Err>.Ok(null);
        }
        
        var r = await searchRepo.SearchUsers(arg, paging);

        if (!r.IsOk)
        {
            return Result<IEnumerable<UserSearchDoc>?, Err>.Err(r.Error);
        }
        if (requester == null) return r;
        return Result<IEnumerable<UserSearchDoc>?, Err>.Ok(await IsFollowMap(requester.GetId(), r.Value));
    }

    public async Task<Result<IEnumerable<UserSearchDoc>?, Err>> RandomUsers(IRequester? requester,int seed, Paging paging)
    {
           
        var r = await searchRepo.RandomUsers(seed, paging);
        
        if (!r.IsOk)
        {
            return Result<IEnumerable<UserSearchDoc>?, Err>.Err(r.Error);
        }
        if (requester == null) return r;
        return Result<IEnumerable<UserSearchDoc>?, Err>.Ok(await IsFollowMap(requester.GetId(), r.Value));
    }

    private async Task<IEnumerable<UserSearchDoc>?> IsFollowMap(int userId,IEnumerable<UserSearchDoc>? users)
    {
        var r = await userRepo.CheckBulkFollow(userId,users.Select(u => u.Id).ToList());
        
        var userList = users.ToList();
        if (!r.IsOk) return users;
        for (var i = 0; i < userList.Count; i++)
        {
            userList[i].IsFollow = r.Value[i];
        }
        return userList;
    }

    public Task<Result<IEnumerable<UserSearchDoc>?, Err>> RecommendUsers(IRequester? requester, Paging paging)
    {
        throw new NotImplementedException();
    }
}