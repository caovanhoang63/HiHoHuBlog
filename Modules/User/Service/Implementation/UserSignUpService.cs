using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;



public class UserSignUpService(IUserRepository userRepo) : IUserSignUpService
{
    public async Task<Result<Unit, Err>> SignUp(UserSignUp u)
    {
        var vR = u.Validate();
        if (!vR.IsOk)
        {
            return Result<Unit,Err>.Err(vR.Error);
        }
        
        var old = await userRepo.FindByEmail(u.Email);

        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }
        
        if (old.Value != null)
        {
            return Result<Unit, Err>.Err(UserErrors.ErrUserAlreadyExists(old.Value.Email));
        }
        
        if (u.Password != u.ConfirmPassword)
            return Result<Unit, Err>.Err(UserErrors.PasswordDoNotMatch());
        
        u.UserName =u.Email;
        u.Salt = Guid.NewGuid().ToString();
        byte[] buffer = Encoding.UTF8.GetBytes(u.Password + u.Salt);
        u.Password=  Encoding.UTF8.GetString(SHA256.HashData(buffer));
        
        var i= await userRepo.Create(u);
        if (!i.IsOk)
        {
            return Result<Unit, Err>.Err(i.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }
}