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
}