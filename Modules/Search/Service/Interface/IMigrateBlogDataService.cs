using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface IMigrateBlogDataService
{
    Task<Result<Unit,Err>> MigrateBlogDataAsync();
}