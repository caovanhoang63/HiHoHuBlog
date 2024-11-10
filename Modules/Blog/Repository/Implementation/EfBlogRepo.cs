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

    public Task<Result<Unit, Err>> DeleteBlog(int id)
    {
        throw new NotImplementedException();
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

    public Task<Result<Entity.Blog, Err>> GetBlogBySlug(string slug)
    {
        throw new NotImplementedException();
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

    public async Task<Result<Unit, Err>> UpdateContent(int id, string content)
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
}