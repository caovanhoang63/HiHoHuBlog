using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface ICreateBlogService
{
    Task<Result<Unit, Err>> CreateNewBlog(IRequester requester, BlogCreate blog);
}