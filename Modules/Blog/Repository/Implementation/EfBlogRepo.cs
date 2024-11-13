using AutoMapper;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog.Modules.Blog.Repository.Implementation;

public class EfBlogRepo : IBlogRepository
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Entity.Blog> _dbSet;
    private IBlogRepository _blogRepositoryImplementation;


    public EfBlogRepo(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
        _dbSet = context.Set<Entity.Blog>();
    }

    public async Task<Result<Unit, Err>> Create(BlogCreate blog)
    {
        try
        {
            var entity = _mapper.Map<Entity.Blog>(blog);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            blog.Id = entity.Id;
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> UpdateMetaData(BlogMetaDataUpdate blog)
    {
        try
        {
            Console.WriteLine(blog.Id);
            var result  = _dbSet.SingleOrDefault(b => b.Id == blog.Id);
            if (result != null)
            {
                var blogProperties = typeof(Entity.Blog).GetProperties();
                var updateProperties = typeof(BlogMetaDataUpdate).GetProperties();

                foreach (var updateProp in updateProperties)
                {
                    var blogProp = blogProperties.FirstOrDefault(p => p.Name == updateProp.Name);
                    if (blogProp != null)
                    {
                        blogProp.SetValue(result, updateProp.GetValue(blog));
                    }
                }
                
                await _context.SaveChangesAsync();
                
                return Result<Unit, Err>.Ok(new Unit());
            }
            return Result<Unit,Err>.Err(UtilErrors.ErrEntityNotFound(nameof(Blog)));
        }
        catch (Exception ex) {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> DeleteBlog(int id)
    {
        try
        {
            await _dbSet.Where(b => b.Id == id)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.Status, 0));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> BanBlog(int id)
    {
        try
        {
            await _dbSet.Where(b => b.Id == id)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.Status, 2));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Entity.Blog?, Err>> GetBlogById(int id)
    {
        try
        {
            var entity = await _dbSet.SingleOrDefaultAsync(b => b.Id == id);
            
            return Result<Entity.Blog?, Err>.Ok(entity);
        }
        catch (Exception e)
        {
            return Result<Entity.Blog?, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<BlogDetail?, Err>> GetBlogDetail(int id)
    {
        try
        {
            var entity = await _dbSet.SingleOrDefaultAsync(b => b.Id == id);
            
            return Result<BlogDetail?, Err>.Ok(_mapper.Map<Entity.Blog?,BlogDetail>(entity));
        }
        catch (Exception e)
        {
            return Result<BlogDetail?, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }


    public async Task<Result<Unit, Err>> UpdateTitle(int id, string title)
    {
        try
        {
            await _dbSet.Where(b => b.Id == id)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.Title, title));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> UpdateContent(int id, string? content)
    {
        try
        {
            await _dbSet.Where(b => b.Id == id)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.Content, content));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public  async Task<Result<IEnumerable<BlogList>?, Err>> GetBlogList(Filter? filter, Paging paging)
    {
        try
        {
            var queryable = _dbSet.AsQueryable();
            if (filter != null)
            {
                if (filter.UserId is not null)
                {
                    queryable = queryable.Where(b => b.UserId == filter.UserId);
                }

                if (filter.Status is not null)
                {
                    queryable = queryable.Where(b => b.Status == filter.Status);
                }

                if (filter.IsPublished is not null)
                {
                    queryable = queryable.Where(b => b.IsPublished == filter.IsPublished);
                }

                if (filter.CreatedAt is not null)
                {
                    queryable = queryable.Where(b => b.CreatedAt >= filter.CreatedAt);
                }

                // if (filter.CategoryId is not null)
                // {
                //     queryable = queryable.Where(b =>)
                // }
            }

            var r = await queryable.OrderByDescending(b=> b.Id).Select(b => _mapper.Map<Entity.Blog, BlogList>(b)).ToListAsync();
            return Result<IEnumerable<BlogList>?, Err>.Ok(r);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<BlogList>?, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> UpdateThumbnail(int id, Image? thumbnail)
    {
        try
        {
            await _dbSet.Where(b => b.Id == id)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.Thumbnail, thumbnail));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> Publish(int id, int minToRead,Image? image)
    {
        try
        {
            await _dbSet.Where(b => b.Id == id)
                .ExecuteUpdateAsync(
                    b => b
                        .SetProperty(u => u.IsPublished, true)
                        .SetProperty(u => u.PublishedAt, DateTime.UtcNow)
                        .SetProperty(u => u.MinToRead, minToRead)
                        .SetProperty(u => u.Thumbnail,image));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> UnPublish(int id)
    {
        try
        {
            await _dbSet.Where(b => b.Id == id)
                .ExecuteUpdateAsync(
                    b => b
                        .SetProperty(u => u.IsPublished, false));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }
}