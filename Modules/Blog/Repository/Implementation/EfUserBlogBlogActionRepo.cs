using AutoMapper;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HiHoHuBlog.Modules.Blog.Repository.Implementation;

public class EfUserBlogActionRepo(ApplicationDbContext context, IMapper mapper) : IUserBlogActionRepository
{

    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<Unit, Err>> ReadBlog(int blogId, int userId)
    {
        try
        {
            var dbSet = _context.Set<UserReadBlog>();
            var old =  await dbSet.SingleOrDefaultAsync(b => b.BlogId == blogId && b.UserId == userId);

            if (old is null)
            {
                await _context.Set<UserReadBlog>().AddAsync(new UserReadBlog { BlogId = blogId, UserId = userId });
            }
            else
            {
                _context.Entry(old).CurrentValues.SetValues(new UserReadBlog
                {
                    BlogId = old.BlogId,
                    UserId = old.UserId,
                    Id = old.Id ,
                    CreatedAt = old.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                });
            }
            await _context.SaveChangesAsync();
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }
    public async Task<Result<IEnumerable<UserReadBlogList>?, Err>> ListReadHistory(int userId, Paging paging)
    {
        try
        {
            var lists =  await _context.Set<UserReadBlog>()
                .Where(b => b.UserId == userId)
                .Where(b => b.Blog.IsPublished == true)
                .Where(b => b.Blog.Status == 1)
                .Skip(paging.GetOffSet())
                .Take(paging.PageSize)
                .OrderByDescending(b  => b.UpdatedAt)
                .Include(b => b.Blog)
                .ThenInclude(b => b.User)
                .Select(b => _mapper.Map<UserReadBlogList>(b)).ToListAsync();

            
            return Result<IEnumerable<UserReadBlogList>?, Err>.Ok(lists);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UserReadBlogList>?, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }
}