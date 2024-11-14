using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface ISearchBlogService
{
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> SearchBlog(string searchTerm, Paging paging);
}