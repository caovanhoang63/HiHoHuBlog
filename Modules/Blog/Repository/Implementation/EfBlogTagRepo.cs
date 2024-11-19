using AutoMapper;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;
using Unit = System.Reactive.Unit;

namespace HiHoHuBlog.Modules.Blog.Repository.Implementation;

public class EfBlogTagRepo(ApplicationDbContext context, IMapper mapper) : IBlogTagRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly DbSet<BlogTag> _dbSet = context.Set<BlogTag>();
    private readonly IMapper _mapper = mapper;


    public  async Task<Result<Unit, Err>> Create(BlogTagCreate b)
    {
        try
        { 
            await _dbSet.AddAsync(_mapper.Map<BlogTag>(b));
            await _context.SaveChangesAsync();
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> DeleteBlogTag(int blogId, int tagId)
    {
        try
        {
            var blogTag = await _dbSet
                .Where(b => b.BlogId == blogId && b.TagId == tagId)
                .FirstOrDefaultAsync();
            if (blogTag != null)
            {
                _dbSet.Remove(blogTag);
                await _context.SaveChangesAsync(); 
            }
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }


}