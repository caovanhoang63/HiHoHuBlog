using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Repository.Interface;

public interface IMigrationRepository
{
    Task<Result<Unit,Err>> AddBulkBlogAsync(IEnumerable<Entity.BlogSearchDoc> blogs, DateTime? date);
    Task<Result<Unit,Err>> AddBulkTagAsync(IEnumerable<Entity.TagSearchDoc> blogs, DateTime? date);
    Task<Result<Unit,Err>> AddBulkUserAsync(IEnumerable<Entity.UserSearchDoc> blogs, DateTime? date);
    Task<Result<MigrationTimestamp?,Err>> GetLastMigrateTime(string index);
}