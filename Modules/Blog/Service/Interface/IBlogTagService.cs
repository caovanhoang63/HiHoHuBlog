using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface IBlogTagService
{
    Task<Result<Unit,Err>> AddTagBlog(BlogTagCreate blogTagCreate);
}