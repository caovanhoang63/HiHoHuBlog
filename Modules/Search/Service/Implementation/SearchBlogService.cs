using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Implementation;
using HiHoHuBlog.Modules.Search.Service.Interface;
using HiHoHuBlog.Utils;
using Quartz.Util;

namespace HiHoHuBlog.Modules.Search.Service.Implementation;

public class SearchBlogService(ISearchBlogRepository searchBlogRepository) : ISearchBlogService
{
    
    private readonly ISearchBlogRepository _searchBlogRepository = searchBlogRepository;

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> SearchBlog(string searchTerm, Paging paging)
    {
        if (searchTerm.IsNullOrWhiteSpace())
        {
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Ok(null);
        }
        
        var r = await _searchBlogRepository.SearchBlog(searchTerm, paging);

        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(r.Error);
        }

        return r;
    }
}