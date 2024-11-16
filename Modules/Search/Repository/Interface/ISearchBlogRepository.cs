using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Repository.Interface;

public interface ISearchBlogRepository
{
    Task<Result<Unit,Err>> AddBulkAsync(IEnumerable<Entity.BlogSearchDoc> blogs, DateTime? date);
    Task<Result<IEnumerable<BlogSearchDoc>?,Err>> SearchBlog(string agrs, Paging paging);
    Task<Result<IEnumerable<BlogSearchDoc>?,Err>> AdminSearchBlog(string agrs,BlogFilter? filter, Paging paging);
}