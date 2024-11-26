using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Interface;

public interface IUserSettingsService
{
    Task<Result<UserSettingsProfile,Err>> GetProfileInformation( string userName);
    Task<Result<Unit,Err>> UpdateProfileInformation(IRequester requester, UserSettingsProfile userSettingsProfile);
}