using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
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

    public Task<Result<IEnumerable<BlogSearchDoc>?, Err>> AdminSearchBlog(string searchTerm, Paging paging)
    {
        throw new NotImplementedException();
    }


    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogByBlog(IRequester? requester,int id, Paging paging)
    {
        var r= await  _searchBlogRepository.RecommendSearchBlogByBlog(requester,id, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(r.Error);
            
        }
        return r;
    }

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogByUser(IRequester? requester,int seed, Paging paging)
    {
        if (requester == null)
            return  Result<IEnumerable<BlogSearchDoc>?, Err>.Err(UtilErrors.ErrNoPermission());
                
        var r= await  _searchBlogRepository.RecommendSearchBlogByUser(requester,seed, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(r.Error);
            
        }
        return r;
    }

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RandomBlog(int seed, Paging paging)
    {
        var r= await  _searchBlogRepository.RandomBlog(seed, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(r.Error);
            
        }
        return r;
        
    }

    public async Task<Result<IEnumerable<BlogSearchDoc>?, Err>> RecommendSearchBlogOfUser(int userId, Paging paging)
    {
        var r= await  _searchBlogRepository.RecommendSearchBlogOfUser(userId, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogSearchDoc>?, Err>.Err(r.Error);
            
        }
        return r;
    }
}