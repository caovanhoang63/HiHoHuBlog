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
    private readonly DbSet<Entity.UserLikeBlog> _dbSetULB = context.Set<Entity.UserLikeBlog>();
    private readonly DbSet<Entity.UserBookmarkBlog> _dbSetUBB = context.Set<Entity.UserBookmarkBlog>();
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
            var blogDetail = mapper.Map<BlogDetail>(entity);
            if (entity?.User != null)
            {
                blogDetail.TotalFollower = entity.User.TotalFollower;
                blogDetail.TotalFollowing = entity.User.TotalFollowing;
            }

            return Result<BlogDetail?, Err>.Ok(blogDetail);
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

                if (filter.Title is not null && filter.Title != "")
                {
                    queryable = queryable.Where(b => b.Title.StartsWith(filter.Title));
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
                    .Include(b => b.Tags)
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
            await UpdateTotalLikes(blogId);
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
         
    }

    public async Task<Result<Unit, Err>> DislikeBlog(int userId, int blogId)
    {
        try
        {
            var entity = await context.UserLikeBlogs
                .FirstOrDefaultAsync(ulb => ulb.UserId == userId && ulb.BlogId == blogId);
            if (entity != null) context.UserLikeBlogs.Remove(entity);
            await context.SaveChangesAsync();
            await UpdateTotalLikes(blogId);
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }
    public async Task<Result<bool, Err>> IsLiked(int userId, int blogId)
    {
        try
        {
            var isLiked = await context.Set<UserLikeBlog>().AnyAsync(ulb => ulb.BlogId == blogId && ulb.UserId == userId);
            return Result<bool, Err>.Ok(isLiked);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<bool, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    private async Task<Result<int?, Err>> GetTotalBookmarks(int blogId)
    {
        try
        {
            int totalBookmarks = await context.UserBookmarkBlogs.CountAsync(ulb => ulb.BlogId == blogId);
            return Result<int?, Err>.Ok(totalBookmarks);
        }
        catch (Exception e)
        {
            return Result<int?, Err>.Err(UtilErrors.InternalServerError(e));

        }
    }
    public async Task<Result<Unit, Err>> UpdateTotalBookmarks(int blogId)
    {
        try
        {
            var totalBookmarksResult = await GetTotalBookmarks(blogId);
            var updateBookMarks = await _dbSet.Where(u=>u.Id==blogId)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.TotalMark, totalBookmarksResult.Value));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }    }

    public async Task<Result<Unit, Err>> BookmarkBlog(int userId, int blogId)
    {
        try
        {
            await context.UserBookmarkBlogs.AddAsync(new UserBookmarkBlog
            {
                BlogId = blogId,
                UserId = userId
            });
            await context.SaveChangesAsync();
            await UpdateTotalBookmarks(blogId);
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<Unit, Err>> UnBookmarkBlog(int userId, int blogId)
    {
        try
        {
            var entity = await context.UserBookmarkBlogs
                .FirstOrDefaultAsync(ulb => ulb.UserId == userId && ulb.BlogId == blogId);
            if (entity != null) context.UserBookmarkBlogs.Remove(entity);
            await context.SaveChangesAsync();
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<bool, Err>> IsBookmarked(int userId, int blogId)
    {
        try
        {
            var isBookmarked = await context.Set<UserBookmarkBlog>().AnyAsync(ulb => ulb.BlogId == blogId && ulb.UserId == userId);
            return Result<bool, Err>.Ok(isBookmarked);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<bool, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<int?, Err>> GetTotalLikes( int blogId)
    {
        try
        {
            int totalLikes = await context.UserLikeBlogs.CountAsync(ulb => ulb.BlogId == blogId);
            return Result<int?, Err>.Ok(totalLikes);
        }
        catch (Exception e)
        {
            return Result<int?, Err>.Err(UtilErrors.InternalServerError(e));

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
            await context.UserCommentBlogs.AddAsync(new UserCommentBlog
            {
                BlogId = blogId,
                UserId = userId,
                Content = content
            });
            await context.SaveChangesAsync();
            await UpdateTotalComments(blogId);
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogListProfile(BlogFilter? filter, Paging paging)
    {
        var r =  await ListBlogs(filter, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<BlogListProfile>?, Err>.Err(r.Error);
        }
        return Result<IEnumerable<BlogListProfile>?, Err>.Ok(mapper.Map<IEnumerable<BlogListProfile>>(r.Value));
    }

    public async Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogListFavorite(BlogFilter? filter, Paging? paging)
    {
        try
        {
            var queryable = _dbSetULB.AsQueryable();
            if (filter != null)
            {
                if (filter.UserId is not null)
                {
                    queryable = queryable.Where(b => b.UserId == filter.UserId);
                }
                
            }

            if (paging is null)
            {
                var result = await queryable
                    .Include(b => b.Blog)
                    .Select(b => b.Blog).ToListAsync();
                return Result<IEnumerable<BlogListProfile>?, Err>.Ok(mapper.Map<IEnumerable<BlogListProfile>>(result));
            }
            
            var totalCount = await queryable.CountAsync();
            paging.Total = totalCount;

            if (paging.Cursor != null)
            {
                queryable =  queryable.OrderByDescending(b=> b.BlogId).Where(b => b.BlogId < paging.Cursor);
            }
            else
            {
                queryable = queryable.OrderByDescending(b=> b.BlogId).Skip(paging.GetOffSet());
            }
            
            var r = await queryable.Take(paging.PageSize)
                .Include(b => b.Blog)
                .Select(b => b.Blog).ToListAsync();

            if (r.Count > 0)
            {
                paging.NextCursor = r[^1]?.Id - 1;
            }
            else
            {
                paging.NextCursor = null;
            }
            
            return Result<IEnumerable<BlogListProfile>?, Err>.Ok(mapper.Map<IEnumerable<BlogListProfile>>(r));
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<BlogListProfile>?, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogListBookmark(BlogFilter? filter, Paging? paging)
    {
        try
        {
            var queryable = _dbSetUBB.AsQueryable();
            if (filter != null)
            {
                if (filter.UserId is not null)
                {
                    queryable = queryable.Where(b => b.UserId == filter.UserId);
                }
                
            }

            if (paging is null)
            {
                var result = await queryable
                    .Include(b => b.Blog)
                    .Select(b => b.Blog).ToListAsync();
                return Result<IEnumerable<BlogListProfile>?, Err>.Ok(mapper.Map<IEnumerable<BlogListProfile>>(result));
            }
            
            var totalCount = await queryable.CountAsync();
            paging.Total = totalCount;

            if (paging.Cursor != null)
            {
                queryable =  queryable.OrderByDescending(b=> b.BlogId).Where(b => b.BlogId < paging.Cursor);
            }
            else
            {
                queryable = queryable.OrderByDescending(b=> b.BlogId).Skip(paging.GetOffSet());
            }
            
            var r = await queryable.Take(paging.PageSize)
                .Include(b => b.Blog)
                .Select(b => b.Blog).ToListAsync();

            if (r.Count > 0)
            {
                paging.NextCursor = r[^1]?.Id - 1;
            }
            else
            {
                paging.NextCursor = null;
            }
            
            return Result<IEnumerable<BlogListProfile>?, Err>.Ok(mapper.Map<IEnumerable<BlogListProfile>>(r));
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<BlogListProfile>?, Err>.Err(UtilErrors.InternalServerError(ex));
        }    
    }

    public async Task<Result<IEnumerable<UserCommentBlog>?, Err>> GetCommentsById(int blogId)
    {
        try
        {
            var comments = await context.UserCommentBlogs
                .Where(ulb => ulb.BlogId == blogId)
                .Include(ulb => ulb.User)
                .Select(ulb => ulb)
                .ToListAsync();
            return Result<IEnumerable<UserCommentBlog>?, Err>.Ok(comments);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<UserCommentBlog>?, Err>.Err(UtilErrors.InternalServerError(e));
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
            return Result<int?, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }
}