using AutoMapper;
using HiHoHuBlog.Modules.Tag.Entity;
using HiHoHuBlog.Modules.Tag.Repository.Interface;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;
using Unit = System.Reactive.Unit;

namespace HiHoHuBlog.Modules.Tag.Repository.Implementation;

public class EfTagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Entity.Tag> _dbSet;
    private IMapper _mapper;

    public EfTagRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _dbSet = _context.Set<Entity.Tag>();
    }

    public async Task<Result<Unit, Err>> Create(TagCreate tag)
    {
        try
        {
            var entity = _mapper.Map<Entity.Tag>(tag);
            await _dbSet.AddAsync(_mapper.Map<Entity.Tag>(entity));
            await _context.SaveChangesAsync();
            tag.Id = entity.Id;
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> Delete(int id)
    {
        try
        { 
            await _dbSet.
                Where(x => x.Id == id).
                ExecuteUpdateAsync( t => t.SetProperty(u => u.Status,0));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<IEnumerable<Entity.Tag>?, Err>> FindWithFilter(TagFilter? tagFilter)
    {
        try
        { 
            var queryable = _dbSet.AsQueryable();
            
            if (tagFilter is not null)
            {
                if (tagFilter.Status is not null)
                {
                    queryable = queryable.Where(t => tagFilter.Status.Contains(t.Status));
                }
                
                if (tagFilter.LtCreatedAt is not null)
                {
                    queryable = queryable.Where(b => b.CreatedAt <= tagFilter.LtCreatedAt);
                }
                if (tagFilter.GtCreatedAt is not null)
                {
                    queryable = queryable.Where(b => b.CreatedAt >= tagFilter.GtCreatedAt);
                }
                
                if (tagFilter.LtUpdatedAt is not null)
                {
                    queryable = queryable.Where(b => b.UpdatedAt <= tagFilter.LtUpdatedAt);
                }
                if (tagFilter.GtUpdatedAt is not null)
                {
                    queryable = queryable.Where(b => b.UpdatedAt >= tagFilter.GtUpdatedAt);
                }
            }
            
            
            var r = await queryable
                .Select(b => b).ToListAsync();
            
            return Result<IEnumerable<Entity.Tag>?, Err>.Ok(r);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Entity.Tag>?, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> UpdateTotalBlogsForTagsAsync()
    {
        try
        {
            var tagCounts = await _context.BlogTags
                .GroupBy(bt => bt.TagId)
                .Select(group => new
                {
                    TagId = group.Key,
                    TotalBlog = group.Count()
                })
                .ToListAsync();

            foreach (var tagCount in tagCounts)
            {
                var tag = await _dbSet.FindAsync(tagCount.TagId);
                if (tag != null)
                {
                    await _dbSet.
                        Where(x => x.Id == tagCount.TagId).
                        ExecuteUpdateAsync( t => t.SetProperty(u => u.TotalBlog,tagCount.TotalBlog));
                }
            }

            await _context.SaveChangesAsync();
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating total blogs for tags: {ex.Message}");
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<IEnumerable<Entity.Tag>?, Err>> ListWithPaging(TagFilter? tagFilter, Paging paging)
    {
        try
        { 
            var queryable = _dbSet.AsQueryable();

            if (tagFilter is not null)
            {
                if (tagFilter.Status is not null)
                {
                    queryable = queryable.Where(t => tagFilter.Status.Contains(t.Status));
                }
            }
            
            paging.Total = await queryable.CountAsync();
            
            if (paging.Cursor != null)
            {
                queryable =  queryable.OrderByDescending(b=> b.Id).Where(b => b.Id < paging.Cursor);
            }
            else
            {
                queryable = queryable.OrderByDescending(b=> b.Id).Skip(paging.GetOffSet());
            }
            
            var r = await queryable.Take(paging.PageSize)
                .Select(b => b).ToListAsync();

            if (r.Count > 0)
            {
                paging.NextCursor = r[^1].Id - 1;
            }
            else
            {
                paging.NextCursor = null;
            }
            
            return Result<IEnumerable<Entity.Tag>?, Err>.Ok(r);
            
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Entity.Tag>?, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    
}