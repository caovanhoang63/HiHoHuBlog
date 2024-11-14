using AutoMapper;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Implementation;
using HiHoHuBlog.Modules.Search.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Service.Implementation;

public class MigrateBlogDataService(IBlogRepository sourceRepository, ISearchBlogRepository targetRepository, IMapper mapper)
    : IMigrateBlogDataService
{
    private readonly IBlogRepository _sourceRepository = sourceRepository;
    private readonly ISearchBlogRepository _targetRepository = targetRepository;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<Unit, Err>> MigrateBlogDataAsync()
    {
        var now = DateTime.UtcNow;
        var lastResult = await _targetRepository.GetLastMigrateTime();
        
        var r = await _sourceRepository.ListBlogs(new BlogFilter
        {
            LtCreatedAt = null,
            GtCreatedAt = null,
            LtUpdatedAt = null,
            GtUpdatedAt = lastResult.Value?.Timestamp,
            IsPublished = null,
            UserId = null
        }, null);
        
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }

        if (r.Value is null || !r.Value.Any())
        {
            Console.WriteLine("No need to migrate blog data");
            return Result<Unit, Err>.Ok(new Unit());
        }
        
        var result = await _targetRepository.AddBulkAsync(_mapper.Map<IEnumerable<BlogSearchDoc>>(r.Value),now);
        
        if (!result.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        
        return Result<Unit, Err>.Ok(new Unit()); 
    }
}