using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface IMigrationSearchDataService
{
    Task<Result<Unit,Err>> MigrateBlogDataAsync();
    Task<Result<Unit,Err>> MigrateTagDataAsync();
    Task<Result<Unit,Err>> MigrateUserDataAsync();
}