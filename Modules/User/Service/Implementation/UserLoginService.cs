using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class UserLoginService(IUserRepository userLoginStore, IMapper mapper) : IUserLoginService
{
    
    public async Task<Result<UserAuth, Err>> Login(UserLogin u)
    {
        var vR = u.Validate();
        if (!vR.IsOk)
        {
            return Result<UserAuth,Err>.Err(vR.Error);
        }
        
        var old = await userLoginStore.FindByEmail(u.Email);
        if (!old.IsOk)
        {
            return Result<UserAuth, Err>.Err(old.Error);
        }

        if (old.Value == null)
        {
            return Result<UserAuth, Err>.Err(UserErrors.ErrInvalidEmailOrPassword());
        }

        byte[] buffer = Encoding.UTF8.GetBytes(u.Password + old.Value.Salt);
        var password=  Encoding.UTF8.GetString(SHA256.HashData(buffer));
        if (old.Value.Password != password)
        {
            return Result<UserAuth, Err>.Err(UserErrors.ErrInvalidEmailOrPassword());
        }

        return Result<UserAuth, Err>.Ok(mapper.Map<UserAuth>(old.Value));
    }
}