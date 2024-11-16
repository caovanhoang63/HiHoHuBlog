using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Repository.Interface;

public interface ISearchTagRepository
{
    Task<Result<IEnumerable<TagSearchDoc>?,Err>> SearchTags(string agrs, Paging paging);
}