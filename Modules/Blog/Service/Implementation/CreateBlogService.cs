using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Implementation;

public class CreateBlogService(IBlogRepository blogRepo ) : ICreateBlogService
{
    public async Task<Result<Unit, Err>> CreateNewBlog(IRequester requester, BlogCreate blog)
    {
        blog.UserId = requester.GetId();

        var r = await blogRepo.Create(blog);
        if (!r.IsOk) return r;
        
        return Result<Unit, Err>.Ok(new Unit());
    }
}