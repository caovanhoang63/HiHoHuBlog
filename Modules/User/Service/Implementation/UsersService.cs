using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class UsersService(IUserRepository userRepository): IUsersService
{
    private async Task<Result<Entity.User?,Err>> UpdateValidate(string email )
    {
        var old = await userRepository.FindByEmail(email);

        if (!old.IsOk)
        {
            return Result<Entity.User?, Err>.Err(old.Error);
        }

        if (old.Value is null || old.Value.Status == 0 )
        {
            return Result<Entity.User?, Err>.Err(UtilErrors.ErrEntityNotFound(nameof(User)));
        }
        return old;
    }
    
    
    public async Task<Result<IEnumerable<UserList>?, Err>> GetUsers(UserFilter? filter, Paging paging)
    {
        var r = await userRepository.GetUserLists(filter, paging);
        if (!r.IsOk)
        {
            return Result<IEnumerable<UserList>?, Err>.Err(r.Error);
        }

        return Result<IEnumerable<UserList>?,Err>.Ok(r.Value);    
    }

    public async Task<Result<Unit, Err>> DeleteUser(IRequester requester, int id)
    {
        if (requester.GetSystemRole() != "admin" && requester.GetSystemRole() != "mod")
            return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        
        var r =  await userRepository.DeleteUser(id);
        
        if (!r.IsOk) return Result<Unit, Err>.Err(r.Error);
        
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> ReActiveUser(IRequester requester, int id)
    {
        if (requester.GetSystemRole() != "admin" && requester.GetSystemRole() != "mod")
            return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        
        var r =  await userRepository.ReActiveUser(id);
        
        if (!r.IsOk) return Result<Unit, Err>.Err(r.Error);
        
        return Result<Unit, Err>.Ok(new Unit());    
    }

    public async Task<Result<Unit, Err>> UpdateRoleUser(IRequester requester, string email, string role)
    {
        var old = await UpdateValidate(email);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }
        
        
    
        // no need to update
        if (old.Value?.Role == role)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }    
        
        var result = await userRepository.UpdateRole(email, role);
        
        return !result.IsOk ? result : Result<Unit, Err>.Ok(new Unit());
    }
}