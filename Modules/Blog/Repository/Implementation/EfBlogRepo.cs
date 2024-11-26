using AutoMapper;
using AutoMapper.QueryableExtensions;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace HiHoHuBlog.Modules.Blog.Repository.Implementation;

public class EfBlogRepo(IMapper mapper, ApplicationDbContext context) : IBlogRepository
{
    private readonly DbSet<Entity.Blog> _dbSet = context.Set<Entity.Blog>();
    public async Task<Result<Unit, Err>> Create(BlogCreate blog)
    {
        try
        {
            var entity = mapper.Map<Entity.Blog>(blog);
            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            
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
                
                await context.SaveChangesAsync();
                
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
            var entity = await _dbSet.Include(b=>b.User).SingleOrDefaultAsync(b => b.Id == id);
            
            return Result<BlogDetail?, Err>.Ok(mapper.Map<Entity.Blog?,BlogDetail>(entity));
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

    
    
    public  async Task<Result<IEnumerable<BlogList>?, Err>> GetBlogList(BlogFilter? filter, Paging paging)
    {
        var r =  await ListBlogs(filter, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogList>?, Err>.Err(r.Error);
        }
        return Result<IEnumerable<BlogList>?, Err>.Ok(mapper.Map<IEnumerable<BlogList>>(r.Value));
    }

    public async Task<Result<IEnumerable<Entity.Blog>?, Err>> ListBlogs(BlogFilter? filter, Paging? paging)
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
                    queryable = queryable.Where(b => filter.Status.Contains(b.Status));
                }

                if (filter.IsPublished is not null)
                {
                    queryable = queryable.Where(b => b.IsPublished == filter.IsPublished);
                }

                if (filter.LtCreatedAt is not null)
                {
                    queryable = queryable.Where(b => b.CreatedAt <= filter.LtCreatedAt);
                }
                if (filter.GtCreatedAt is not null)
                {
                    queryable = queryable.Where(b => b.CreatedAt >= filter.GtCreatedAt);
                }
                
                if (filter.LtUpdatedAt is not null)
                {
                    queryable = queryable.Where(b => b.UpdatedAt <= filter.LtUpdatedAt);
                }
                if (filter.GtUpdatedAt is not null)
                {
                    queryable = queryable.Where(b => b.UpdatedAt >= filter.GtUpdatedAt);
                }


                // if (filter.CategoryId is not null)
                // {
                //     queryable = queryable.Where(b =>)
                // }
            }

            if (paging is null)
            {
                var result = await queryable
                    .Include(b => b.User)
                    .Select(b => b).ToListAsync();
                return Result<IEnumerable<Entity.Blog>?, Err>.Ok(result);
            }
            
            var totalCount = await queryable.CountAsync();
            paging.Total = totalCount;

            if (paging.Cursor != null)
            {
                queryable =  queryable.OrderByDescending(b=> b.Id).Where(b => b.Id < paging.Cursor);
            }
            else
            {
                queryable = queryable.OrderByDescending(b=> b.Id).Skip(paging.GetOffSet());
            }
            
            var r = await queryable.Take(paging.PageSize)
                .Include(b => b.User)
                .Select(b => b).ToListAsync();

            if (r.Count > 0)
            {
                paging.NextCursor = r[^1].Id - 1;
            }
            else
            {
                paging.NextCursor = null;
            }
            
            return Result<IEnumerable<Entity.Blog>?, Err>.Ok(r);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Entity.Blog>?, Err>.Err(UtilErrors.InternalServerError(ex));
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

    public async Task<Result<Unit, Err>> UpdateTotalLikes(int id)
    {
        try
        {
            var totalLikesResult = await GetTotalLikes(id);
            var updateTotalLikes = await _dbSet.Where(u=>u.Id==id)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.TotalLike, totalLikesResult.Value));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<Unit, Err>> LikeBlog(int userId, int blogId)
    {
        try
        {
            await context.UserLikeBlogs.AddAsync(new UserLikeBlog
            {
                BlogId = blogId,
                UserId = userId
            });
            await context.SaveChangesAsync();
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
         
    }

    private async Task<Result<int?, Err>> GetTotalLikes( int blogId)
    {
        try
        {
            int totalLikes = await context.UserLikeBlogs.CountAsync(ulb => ulb.BlogId == blogId);
            return Result<int?, Err>.Ok(totalLikes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<Result<Unit, Err>> UpdateTotalComments(int id)
    {
        try
        {
            var totalCommentsResult = await GetTotalComments(id);
            var updateTotalComments = await _dbSet.Where(u=>u.Id==id)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.TotalComment, totalCommentsResult.Value));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<Unit, Err>> Comment(int userId, int blogId, string content)
    {
        try
        {
            await context.UserCommentBlogs.AddAsync(new UserCommentBlog()
            {
                BlogId = blogId,
                UserId = userId,
                Content = content
            });
            await context.SaveChangesAsync();
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }
    private async Task<Result<int?, Err>> GetTotalComments( int blogId)
    {
        try
        {
            int totalComments = await context.UserCommentBlogs.CountAsync(ulb => ulb.BlogId == blogId);
            return Result<int?, Err>.Ok(totalComments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}