using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Repository.Implementation;

public interface ISearchBlogRepository
{
    Task<Result<Unit,Err>> AddBulkAsync(IEnumerable<Entity.BlogSearchDoc> blogs, DateTime? date);
    Task<Result<MigrationTimestamp?,Err>> GetLastMigrateTime();
    Task<Result<IEnumerable<BlogSearchDoc>?,Err>> SearchBlog(string agrs, Paging paging);
}