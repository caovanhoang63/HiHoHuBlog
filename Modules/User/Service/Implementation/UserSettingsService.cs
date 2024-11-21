using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class UserSettingsService(IUserRepository userRepository): IUserSettingsService
{
    private async Task<Result<UserSettingsProfile?,Err>> UpdateSettingsProfileValidate(IRequester requester)
    {
        var old = await userRepository.GetSettingsProfile(requester.GetUsername());

        if (!old.IsOk)
        {
            return Result<UserSettingsProfile?, Err>.Err(old.Error);
        }

        if (old.Value is null)
        {
            return Result<UserSettingsProfile?, Err>.Err(UtilErrors.ErrEntityNotFound(nameof(UserSettingsProfile)));
        }
        
        return old;
    }
    
    public async Task<Result<UserSettingsProfile?, Err>> GetProfileInformation(string userName)
    {
        var r = await userRepository.GetSettingsProfile(userName);

        if (!r.IsOk)
        {
            return Result<UserSettingsProfile?, Err>.Err(r.Error);
        }

        if (r.Value is null)
        {
            return Result<UserSettingsProfile?, Err>.Err(UtilErrors.ErrEntityNotFound("user"));
        }
        
        return Result<UserSettingsProfile?, Err>.Ok(r.Value);
    }

    public async Task<Result<Unit, Err>> UpdateProfileInformation(IRequester requester,UserSettingsProfile userSettingsProfile)
    {
        var vR = userSettingsProfile.Validate();
        if (!vR.IsOk)
        {
            return Result<Unit,Err>.Err(vR.Error);
        }
        var vS = await UpdateSettingsProfileValidate(requester);
        if (!vS.IsOk)
        {
            return Result<Unit,Err>.Err(vR.Error);
        }
        
        // no need to update
        if (vS.Value.FirstName == userSettingsProfile.FirstName &&
            vS.Value.LastName == userSettingsProfile.LastName &&
            vS.Value.Avatar?.Url == userSettingsProfile.Avatar?.Url &&
            vS.Value.ShortBio == userSettingsProfile.ShortBio)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }
        var r = await userRepository.UpdateSettingsProfile(userSettingsProfile);
        return !r.IsOk ? Result<Unit,Err>.Err(UtilErrors.InternalServerError(null)) : Result<Unit, Err>.Ok(new Unit());
    }
}