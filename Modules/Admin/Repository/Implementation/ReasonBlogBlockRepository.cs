using AutoMapper;
using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Modules.Admin.Repository.Interface;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog.Modules.Admin.Repository.Implementation;

public class ReasonBlogBlockRepository(ApplicationDbContext context, Mapper mapper) : IReasonBlogBlockRepository
{
    private readonly DbSet<ReasonBlogBlock> _dbSet = context.Set<ReasonBlogBlock>();


    public async Task<Result<Unit, Err>> Create(ReasonBlogBlockCreate data)
    {
        try
        {
            await _dbSet.AddAsync(mapper.Map<ReasonBlogBlock>(data));
            await context.SaveChangesAsync();
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

  

    public async Task<Result<IEnumerable<ReasonBlogBlock>, Err>> ListAll()
    {
        try
        {  
            var r =  await _dbSet.Select(b => b).ToListAsync();
            return Result<IEnumerable<ReasonBlogBlock>, Err>.Ok(r);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ReasonBlogBlock>, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }
}