using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Implementation;

public class BlogGetService(IBlogRepository blogRepo) : IBlogGetService
{
    public async Task<Result<BlogDetail?, Err>> GetBlog(int id)
    {
       var r = await blogRepo.GetBlogDetail(id);

       if (!r.IsOk)
       {
           return Result<BlogDetail?, Err>.Err(r.Error);
       }

       if (r.Value is not { Status: (int)Status.Active })
       {
           return Result<BlogDetail?, Err>.Err(UtilErrors.ErrEntityNotFound("blog"));
       }
       
       r.Value.Slug = SlugHelper.GenerateSlug(r.Value.Title) + '-' + id ;
       
       return r.Value;
    }

    public async Task<Result<IEnumerable<BlogList>?, Err>> GetBlogs(Filter? filter, Paging paging)
    {
        var r = await blogRepo.GetBlogList(filter, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogList>?, Err>.Err(r.Error);
        }

        return r;
    }
    
}