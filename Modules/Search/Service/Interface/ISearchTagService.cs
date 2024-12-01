using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Interface;

public interface ISearchTagService
{
    Task<Result<IEnumerable<TagSearchDoc>?, Err>> SearchTags(string arg, Paging paging);
    Task<Result<IEnumerable<TagSearchDoc>?, Err>> SearchTagsForPublish(string arg, Paging paging);
    Task<Result<IEnumerable<TagSearchDoc>?,Err>> RandomSearchTags(int seed, Paging paging);

}