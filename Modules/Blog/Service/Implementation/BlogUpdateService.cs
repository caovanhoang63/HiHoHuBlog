using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Implementation;

public class BlogUpdateService(IBlogRepository blogRepo) : IBlogUpdateService
{
    private readonly IBlogRepository _blogRepo = blogRepo;

    public async Task<Result<Unit, Err>> UpdateTitle(IRequester requester, int id, string title)
    {
        var old = await blogRepo.GetBlogById(id);

        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }

        if (old.Value is null || old.Value.Status == 0 )
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound(nameof(Blog)));
        }

        if (old.Value.UserId != requester.GetId())
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        }

        // no need to update
        if (old.Value.Title == title)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }
        
        var result = await blogRepo.UpdateTitle(id, title);
        
        return !result.IsOk ? result : Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> UpdateContent(IRequester requester, int id, string content)
    {
        var old = await blogRepo.GetBlogById(id);

        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }

        if (old.Value is null || old.Value.Status == 0)
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound(nameof(Blog)));
        }

        if (old.Value.UserId != requester.GetId())
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        }

        
        var result = await blogRepo.UpdateContent(id, content);
        
        return !result.IsOk ? result : Result<Unit, Err>.Ok(new Unit());
    }
}