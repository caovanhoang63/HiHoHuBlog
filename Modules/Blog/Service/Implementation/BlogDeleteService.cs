using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Utils;
using Unit = System.Reactive.Unit;

namespace HiHoHuBlog.Modules.Blog.Service.Implementation;

public class BlogDeleteService(IBlogRepository blogRepository) : IBlogDeleteService
{
    private IBlogDeleteService _blogDeleteServiceImplementation;

    public async Task<Result<Unit, Err>> Delete(IRequester requester,int id)
    {
        var old = await blogRepository.GetBlogById(id);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }

        if (old.Value is null)
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("blog"));
        }

        if (old.Value.UserId != requester.GetId() && requester.GetSystemRole() != "admin" &&
            requester.GetSystemRole() != "mod")
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        }

        var r = await blogRepository.DeleteBlog(id);
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());

    }

    public async Task<Result<Unit, Err>> Block(IRequester requester, int id)
    {
        var old = await blogRepository.GetBlogById(id);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }

        if (old.Value is null)
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("blog"));
        }

        if ( requester.GetSystemRole() != "admin" &&
            requester.GetSystemRole() != "mod")
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        }

        var r = await blogRepository.BanBlog(id);
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }
}