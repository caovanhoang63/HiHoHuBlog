using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Implementation;

public class UserBlogActionService(IUserBlogActionRepository userBlogRepo) : IUserBlogActionService
{
    
    
    private readonly IUserBlogActionRepository _userBlogRepo = userBlogRepo;

    public async Task<Result<Unit, Err>> ReadBlog(IRequester requester, int blogId)
    {
        
        var result =  await _userBlogRepo.ReadBlog( blogId,requester.GetId());
        if (result.IsOk)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }
        return Result<Unit, Err>.Err(result.Error);
    }

    public async Task<Result<IEnumerable<UserReadBlogList>?, Err>> ListReadHistory(IRequester requester, int userId, Paging paging)
    {
        if (requester.GetId() != userId)
            return Result<IEnumerable<UserReadBlogList>?, Err>.Err(UtilErrors.ErrNoPermission());

        var result =  await _userBlogRepo.ListReadHistory(userId, paging);
        if (result.IsOk)
        {
            return Result<IEnumerable<UserReadBlogList>?, Err>.Ok(result.Value);

        }
        return Result<IEnumerable<UserReadBlogList>?, Err>.Err(result.Error);
    }
}