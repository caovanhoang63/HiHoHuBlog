using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Implementation;

public class BlogGetService(IBlogRepository blogRepo) : IBlogGetService
{
    public async Task<Result<BlogDetail?, Err>> GetBlog(IRequester? requester, int id)
    {
       var r = await blogRepo.GetBlogDetail(id);

       if (!r.IsOk)
       {
           return Result<BlogDetail?, Err>.Err(r.Error);
       }

       if (r.Value is null)
       {
           return Result<BlogDetail?, Err>.Err(UtilErrors.ErrEntityNotFound("blog"));
       }

       // other user can't see deleted and blocked blogs
       if (r.Value.Status != (int)Status.Active && 
           (requester is null || (requester.GetId() != r.Value.UserId && requester.GetSystemRole() == "user")))
       {
           return Result<BlogDetail?, Err>.Err(UtilErrors.ErrEntityNotFound("blog"));
       }

       // authors can see their blocked blogs but can't see deleted blogs
       if (r.Value.Status == (int)Status.Deleted
           && (requester?.GetId() != r.Value.UserId || requester?.GetSystemRole() == "user"))
       {
           Console.WriteLine(123);
           return Result<BlogDetail?, Err>.Err(UtilErrors.ErrEntityNotFound("blog"));
       }
       
       
       r.Value.Slug = SlugHelper.GenerateSlug(r.Value.Title) + '-' + id  ;
       
       return r.Value;
    }

    public async Task<Result<IEnumerable<BlogList>?, Err>> GetBlogs(BlogFilter? filter, Paging paging)
    {
        var r = await blogRepo.GetBlogList(filter, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogList>?, Err>.Err(r.Error);
        }

        return r;
    }

    public async Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogProfiles(BlogFilter? filter, Paging paging)
    {
        var r = await blogRepo.GetBlogListProfile(filter, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogListProfile>?, Err>.Err(r.Error);
        }

        return r;
    }

    public async Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogsFavorite(BlogFilter? filter, Paging paging)
    {
        var r = await blogRepo.GetBlogListFavorite(filter, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogListProfile>?, Err>.Err(r.Error);
        }

        return r;
    }
}