using AutoMapper;
using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Modules.Admin.Repository.Interface;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog.Modules.Admin.Repository.Implementation;

public class BlogBlockedRepository(ApplicationDbContext context, Mapper mapper) : IBlogBlockedRepository
{
    private readonly DbSet<BlogBlocked> _dbSet = context.Set<BlogBlocked>();

    public async Task<Result<Unit, Err>> Create(BlogBlockedCreate data)
    {
        try
        {
             await _dbSet.AddAsync(mapper.Map<BlogBlocked>(data));
             await context.SaveChangesAsync();
             return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }
}