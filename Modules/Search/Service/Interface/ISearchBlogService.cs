using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface ISearchBlogService
{
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> SearchBlog(string searchTerm, Paging paging);
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> AdminSearchBlog(string searchTerm, Paging paging);

    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogByBlog(IRequester? requester, int id,
        Paging paging);

    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogByUser(IRequester? requester,int seed, Paging paging);

    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RandomBlog(int seed, Paging paging,BlogFilter blogFilter = null);
    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogOfUser(int userId, Paging paging);
}