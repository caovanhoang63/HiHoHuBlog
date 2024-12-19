using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;
using Nest;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface IBlogGetService
{
    Task<Result<BlogDetail?,Err>> GetBlog(IRequester? requester,int id);
    Task<Result<IEnumerable<BlogList>?, Err>> GetBlogs(BlogFilter? filter, Paging paging);
    Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogProfiles(BlogFilter? filter, Paging paging);
    Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogsFavorite(BlogFilter? filter, Paging paging);
}