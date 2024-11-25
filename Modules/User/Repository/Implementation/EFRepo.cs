using AutoMapper;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Repository.Implementation;

public class EfRepo  : IUserRepository
{
    private readonly DbSet<Entity.User> _dbSet;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EfRepo( ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _dbSet = _dbContext.Set<Entity.User>();
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
    
    public async Task<Result<Entity.User?, Err>> FindByEmailAndUserName(string email,string userName)
    {
        var user = await _dbSet.Where(u => u.Email == email )
            .Where(u => u.UserName == userName).FirstOrDefaultAsync();

        return Result<Entity.User?, Err>.Ok(user);
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
    public async Task<Result<Unit, Err>> UpdateTotalFollows(int id)
    {
        try
        {
            var totalFollowsResult = await GetTotalFollows(id);
            var updateTotalFollows = await _dbSet.Where(u=>u.Id==id)
                .ExecuteUpdateAsync(
                    b => b.SetProperty(u => u.TotalFollower, totalFollowsResult.Value));
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
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception e)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(e));
        }
         
    }

    private async Task<Result<int?, Err>> GetTotalFollows(int id)
    {
        try
        {
            int totalFollows = await _dbContext.UserFollows.CountAsync(ulb => ulb.UserFollowing == id);
            return Result<int?, Err>.Ok(totalFollows);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}