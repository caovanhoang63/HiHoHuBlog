using HiHoHuBlog.Modules.User.Biz;
using HiHoHuBlog.modules.user.model;
using HiHoHuBlog.Modules.User.Model;
using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog.Modules.User.Data;

// ReSharper disable once InconsistentNaming
public class EFData  : IUserSignUpStore, IUserLoginStore
{
    private ApplicationDbContext DbContext { get; }
    public EFData(ApplicationDbContext dbContext)
    {
        this.DbContext = dbContext;
    }
    public async Task UserSignUp(UserSignUp userSignUp)
    {
        await DbContext.Set<UserSignUp>().AddAsync(userSignUp);
    }

    public async Task Login(UserLogin userLogin)
    {
        await DbContext.Set<UserLogin>().Where(predicate: u => u.Email == userLogin.Email).FirstAsync();
    }
}