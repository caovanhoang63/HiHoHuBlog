using AutoMapper;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;
using HiHoHuBlog.Utils;
using Newtonsoft.Json;

namespace HiHoHuBlog.Modules.User.Repository.Implementation;

public class EfRepo  : IUserRepository
{
    private readonly DbSet<Entity.User> _dbSet;
    private readonly DbSet<Entity.UserDetails> _dbDetailsSet;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public EfRepo( ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dbSet = _dbContext.Set<Entity.User>();
        _dbDetailsSet = _dbContext.Set<Entity.UserDetails>();
    }

    public async Task<Result<Unit, Err>> Create(UserSignUp userSignUp)
    {
        var entity = _mapper.Map<Entity.User>(userSignUp);

        await _dbSet.AddAsync(entity);
        var saveResult = await _dbContext.SaveChangesAsync();

        if (saveResult <= 0) 
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(null));
        }

        userSignUp.Id = entity.Id;
        return Result<Unit, Err>.Ok(new Unit()); 
    }

    public async Task<Result<Entity.User?, Err>> FindByEmail(string email)
    {
        var user = await _dbSet.Where(u => u.Email == email).FirstOrDefaultAsync();

        return Result<Entity.User?, Err>.Ok(user);
    }

    public async Task<Result<IEnumerable<Entity.User>, Err>> ListUsers(UserFilter? filter, Paging? paging)
    {
        var queryable = _dbSet.AsQueryable();
        if (filter != null)
        {
            if (filter.Status is not null)
            {
                queryable = queryable.Where(b => filter.Status.Contains(b.Status));
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
        }

        if (paging is not null)
        {
            queryable =  queryable.Skip(paging.GetOffSet())
                .Take(paging.PageSize);
        }
        
        try
        {
            var r = await queryable
                .Include(b => b.UserDetails)
                .Select(b => b).ToListAsync();
            
            return Result<IEnumerable<Entity.User>, Err>.Ok(r);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Entity.User>, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Entity.User?, Err>> FindByEmailAndUserName(string email,string userName)
    {
        var user = await _dbSet.Where(u => u.Email == email )
            .Where(u => u.UserName == userName).FirstOrDefaultAsync();

        return Result<Entity.User?, Err>.Ok(user);
    }

    public async Task<Result<Unit, Err>> UpdateSettingsProfile(UserSettingsProfile userSettingsProfile)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var userUpdateResult = await _dbSet
                .Where(u => u.Id == userSettingsProfile.Id)
                .ExecuteUpdateAsync(
                    u => u
                        .SetProperty(i => i.Avatar, userSettingsProfile.Avatar)
                        .SetProperty(i => i.FirstName, userSettingsProfile.FirstName)
                        .SetProperty(i => i.LastName, userSettingsProfile.LastName)
                );

            var userDetails = await _dbDetailsSet
                .Where(ud => ud.Id == userSettingsProfile.Id)
                .FirstOrDefaultAsync();

            if (userDetails != null)
            {
                var userDetailsUpdateResult =await _dbDetailsSet
                    .Where(ud => ud.Id == userSettingsProfile.Id)
                    .ExecuteUpdateAsync(
                        ud => ud
                            .SetProperty(i => i.Bio, userSettingsProfile.ShortBio)
                            .SetProperty(i => i.Status, 1)
                    );
                
                if (userUpdateResult > 0&& userDetailsUpdateResult>0)
                {
                    await transaction.CommitAsync();
                    Console.WriteLine("Cap nhap thanh cong 2");
                    return Result<Unit, Err>.Ok(new Unit());
                }
                else
                {            
                    await transaction.RollbackAsync();
                    return Result<Unit, Err>.Err(UtilErrors.InternalServerError(null));
                }
            }
            else
            {
                var newUserDetails = new UserDetails
                {
                    Id = userSettingsProfile.Id,
                    Bio = userSettingsProfile.ShortBio,
                };
                await _dbDetailsSet.AddAsync(newUserDetails);
                var saveResult = await _dbContext.SaveChangesAsync();
                Console.WriteLine(saveResult.ToString());

                if (saveResult > 0)
                {
                    Console.WriteLine("Cap nhap thanh cong 2");
                    await transaction.CommitAsync();
                    return Result<Unit, Err>.Ok(new Unit());
                }
                else
                {            
                    await transaction.RollbackAsync();
                    return Result<Unit, Err>.Err(UtilErrors.InternalServerError(null));
                }
            }
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<UserSettingsProfile?, Err>> GetSettingsProfile(string userName)
    {
        try
        {
            var userSettingsProfile = await  (
                    from u in _dbSet
                    join ud in _dbDetailsSet on u.Id equals ud.UserId into detailsGroup
                    from ud in detailsGroup.DefaultIfEmpty()
                    where (u.UserName == userName)
                        select new UserSettingsProfile()
                        {
                            Id = u.Id,
                            Avatar = u.Avatar,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            ShortBio = ud != null ? ud.Bio : null,
                        }
                    
                ).FirstOrDefaultAsync();
            
            if (userSettingsProfile == null)
            {
                return Result<UserSettingsProfile?, Err>.Err(UtilErrors.ErrEntityNotFound(nameof(UserSettingsProfile)));
            }

            return Result<UserSettingsProfile?, Err>.Ok(userSettingsProfile);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Result<UserSettingsProfile?, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<Unit, Err>> UpdatePassword(string email, string password)
    {
        try
        {
            await _dbSet.Where(u => u.Email == email)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(i => i.Password, password)
                );
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }    
    }


    public async Task<Result<Entity.User?, Err>> FindById(int id)
    {
        try
        {
            var user = await _dbSet.Where(u => u.Id == id).FirstOrDefaultAsync();
            return Result<Entity.User?, Err>.Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<Entity.User?, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<IEnumerable<UserList>?, Err>> GetUserLists(UserFilter? userFilter, Paging? paging)
    {
        var r =  await ListUsers(userFilter, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<UserList>?, Err>.Err(r.Error);
        }
        return Result<IEnumerable<UserList>?, Err>.Ok(_mapper.Map<IEnumerable<UserList>>(r.Value));    }

    public async Task<Result<Unit, Err>> DeleteUser(int id)
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

    public async Task<Result<Unit, Err>> ReActiveUser(int id)
    {
        try
        { 
            await _dbSet.
                Where(x => x.Id == id).
                ExecuteUpdateAsync( t => t.SetProperty(u => u.Status,1));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }    }

    public async Task<Result<Unit, Err>> UpdateRole(string email, string role)
    {
        try
        {
            await _dbSet.Where(b => b.Email == email)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.Role, role));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));

        }
    }

    public async Task<Result<bool[], Err>> CheckBulkFollow(int userId, List<int> userFollowIds)
    {
        try
        {
            var followedIds = await _dbContext.UserFollows
                .Where(uf => uf.UserId == userId && userFollowIds.Contains(uf.UserFollowing))
                .Select(uf => uf.UserFollowing)
                .ToListAsync();

            var result = userFollowIds.Select(id => followedIds.Contains(id)).ToArray();
            return Result<bool[], Err>.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<bool[], Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<UserProfile?, Err>> GetProfile(string userName)
    {
        try
        {
            var user = await _dbSet.Where(u => u.UserName == userName).FirstOrDefaultAsync();
            var userProfile = _mapper.Map<UserProfile>(user);
            return Result<UserProfile?, Err>.Ok(userProfile);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<UserProfile?, Err>.Err(UtilErrors.InternalServerError(e));
        }
        
    }
    public async Task<Result<Unit, Err>> UpdateTotalFollows(int id,int userFollowId)
    {
        try
        {
            var totalFollowsResult = await GetTotalFollows(id);
            var totalFollowingResult = await GetTotalFollowing(userFollowId);
            var updateTotalFollowing = await _dbSet.Where(u => u.Id == id)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(u => u.TotalFollowing, totalFollowsResult.Value));
            var updateTotalFollower = await _dbSet.Where(u => u.Id == userFollowId)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(u => u.TotalFollower, totalFollowingResult.Value));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<Unit, Err>> Follows(int userId, int userFollowId)
    {
        try
        {
            await _dbContext.UserFollows.AddAsync(new UserFollow
            {
                UserId = userId,
                UserFollowing = userFollowId
            });
            await _dbContext.SaveChangesAsync();
            await UpdateTotalFollows(userId,userFollowId);
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
         
    }

    public async Task<Result<Unit, Err>> UnFollow(int userId, int userFollowId)
    {
        try
        {
            var entity = await _dbContext.UserFollows
                .FirstOrDefaultAsync(ulb => ulb.UserId == userId && ulb.UserFollowing == userFollowId);
            if (entity != null) _dbContext.UserFollows.Remove(entity);
            await _dbContext.SaveChangesAsync();
            await UpdateTotalFollows(userId,userFollowId);
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
    }

    public async Task<Result<bool, Err>> IsFollowed(int userId, int userFollowId)
    {
        try
        {
            var isFollowed = await _dbContext.UserFollows.AnyAsync(uf => uf.UserId == userId && uf.UserFollowing == userFollowId);
            return Result<bool, Err>.Ok(isFollowed);
        }
        catch (Exception e)
        {
            return Result<bool, Err>.Err(UtilErrors.InternalServerError(e));

        }
    }
    private async Task<Result<int?, Err>> GetTotalFollows(int id)
    {
        try
        {
            int totalFollows = await _dbContext.UserFollows.CountAsync(ulb => ulb.UserId == id);
            return Result<int?, Err>.Ok(totalFollows);
        }
        catch (Exception e)
        {
            return Result<int?, Err>.Err(UtilErrors.InternalServerError(e));

        }
    }
    private async Task<Result<int?, Err>> GetTotalFollowing(int id)
    {
        try
        {
            int totalFollows = await _dbContext.UserFollows.CountAsync(ulb => ulb.UserFollowing == id);
            return Result<int?, Err>.Ok(totalFollows);
        }
        catch (Exception e)
        {
            return Result<int?, Err>.Err(UtilErrors.InternalServerError(e));

        }
    }
    
    
}