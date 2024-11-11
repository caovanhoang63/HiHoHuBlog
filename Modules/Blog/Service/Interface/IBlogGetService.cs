using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface IBlogGetService
{
    Task<Result<BlogDetail?,Err>> GetBlog(int id);
}