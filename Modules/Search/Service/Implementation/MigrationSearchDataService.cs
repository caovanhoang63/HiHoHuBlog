using AutoMapper;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Search.Entity;
using HiHoHuBlog.Modules.Search.Repository.Interface;
using HiHoHuBlog.Modules.Search.Service.Interface;
using HiHoHuBlog.Modules.Tag.Entity;
using HiHoHuBlog.Modules.Tag.Repository.Interface;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Utils;
using HtmlAgilityPack;

namespace HiHoHuBlog.Modules.Search.Service.Implementation;

public class MigrationSearchDataService(IBlogRepository blogRepository, IMapper mapper, IUserRepository userRepository, ITagRepository tagRepository, IMigrationRepository targetRepository1, IMigrationRepository targetRepository)
    : IMigrationSearchDataService
{
    private readonly IBlogRepository _blogRepository = blogRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IMigrationRepository _targetRepository = targetRepository; 
    private readonly IMapper _mapper = mapper;
    public async Task<Result<Unit, Err>> MigrateBlogDataAsync()
    {
        var now = DateTime.UtcNow;
        var lastResult = await _targetRepository.GetLastMigrateTime("blog_search");
        
        var r = await _blogRepository.ListBlogs(new BlogFilter
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

        var blogs = _mapper.Map<IEnumerable<BlogSearchDoc>>(r.Value);
        var blogSearchDocs = blogs as BlogSearchDoc[] ?? blogs.ToArray();
        foreach (var blog in blogSearchDocs)
        {
            var doc = new HtmlDocument();
            if (blog.Content is not null)
            {
                doc.LoadHtml(blog.Content);
                blog.Content = doc.DocumentNode.InnerText;
                var firstTextElement = doc.DocumentNode.SelectSingleNode("//h1|//h2|//h3|//p");
                if (firstTextElement != null)
                {
                    blog.ShortContent = firstTextElement.InnerText;
                }
            }
        }
        
        var result = await _targetRepository.AddBulkBlogAsync(blogSearchDocs,now);
        
        if (!result.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        
        return Result<Unit, Err>.Ok(new Unit()); 
    }

    public async Task<Result<Unit, Err>> MigrateTagDataAsync()
    {
        var now = DateTime.UtcNow;
        var lastResult = await _targetRepository.GetLastMigrateTime("tag_search");
        var r = await _tagRepository.FindWithFilter(new TagFilter
        {
            GtUpdatedAt = lastResult.Value?.Timestamp,
        });
        
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        
        if (r.Value is null || !r.Value.Any())
        {
            Console.WriteLine("No need to migrate blog data");
            return Result<Unit, Err>.Ok(new Unit());
        }
        
        var result = await _targetRepository.AddBulkTagAsync(_mapper.Map<IEnumerable<TagSearchDoc>>(r.Value),now);
        
        if (!result.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> MigrateUserDataAsync()
    {
        var now = DateTime.UtcNow;
        var lastResult = await _targetRepository.GetLastMigrateTime("user_search");
        var r = await _userRepository.ListUsers(new UserFilter
        {
            GtUpdatedAt = lastResult.Value?.Timestamp,
        },null);
        
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        
        if (!r.Value.Any())
        {
            Console.WriteLine("No need to migrate blog data");
            return Result<Unit, Err>.Ok(new Unit());
        }
        
        var result = await _targetRepository.AddBulkUserAsync(_mapper.Map<IEnumerable<UserSearchDoc>>(r.Value),now);
        
        if (!result.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        
        return Result<Unit, Err>.Ok(new Unit());
    }
}