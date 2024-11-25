using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Modules.Search.Service.Interface;
using HiHoHuBlog.Utils;
using Quartz.Util;

namespace HiHoHuBlog.Modules.Search.Service.Implementation;

public class SearchTagService(ISearchTagRepository searchTagRepository) : ISearchTagService
{
    private readonly ISearchTagRepository _searchTagRepository = searchTagRepository;

    public async Task<Result<IEnumerable<TagSearchDoc>?, Err>> SearchTags(string arg, Paging paging)
    {
        if (arg.IsNullOrWhiteSpace())
        {
            return Result<IEnumerable<TagSearchDoc>?, Err>.Ok(null);
        }
        
        var r = await _searchTagRepository.SearchTags(arg, paging);

        if (!r.IsOk)
        {
            return Result<IEnumerable<TagSearchDoc>?, Err>.Err(r.Error);
        }

        return r;
    }

    public async Task<Result<IEnumerable<TagSearchDoc>?, Err>> SearchTagsForPublish(string arg, Paging paging)
    {
        if (arg.IsNullOrWhiteSpace())
        {
            return Result<IEnumerable<TagSearchDoc>?, Err>.Ok(null);
        }
        
        var r = await _searchTagRepository.SearchTagsForPublish(arg, paging);

        if (!r.IsOk)
        {
            return Result<IEnumerable<TagSearchDoc>?, Err>.Err(r.Error);
        }

        return r;
    }
}