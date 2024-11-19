using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface ISearchBlogService
{
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> SearchBlog(string searchTerm, Paging paging);
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> AdminSearchBlog(string searchTerm, Paging paging);
    
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogByBlog(IRequester? requester, BlogSearchDoc searchDoc,Paging paging);
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogByUser(IRequester? requester,Paging paging);
    
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RandomBlog(int seed, Paging paging);
}