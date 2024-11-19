using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;
using Nest;

namespace HiHoHuBlog.Modules.Search.Repository.Interface;

public interface ISearchBlogRepository
{
    Task<Result<IEnumerable<BlogSearchDoc>?,Err>> SearchBlog(string agrs, Paging paging);
    Task<Result<IEnumerable<BlogSearchDoc>?,Err>> AdminSearchBlog(string agrs,BlogFilter? filter, Paging paging);
    Task<Result<IEnumerable<BlogSearchDoc>?,Err>> RecommendSearchBlogByUser(IRequester requester , Paging paging);
    Task<Result<IEnumerable<BlogSearchDoc>?,Err>> RecommendSearchBlogByBlog(IRequester? requester,int id , Paging paging);

    Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RandomBlog(int seed, Paging paging);
}