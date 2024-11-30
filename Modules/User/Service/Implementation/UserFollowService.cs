using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Service.Implementation;

public class UserFollowService(IUserRepository userRepository):IUserFollowService
{
    private readonly IUserRepository _userRepository = userRepository;
    private IUserFollowService _userFollowServiceImplementation;
    public async Task<Result<Unit, Err>> UpdateTotalFollows(int id,int userFollowId)
    {
        var _totalFollow = await _userRepository.UpdateTotalFollows(id,userFollowId);
        if (!_totalFollow.IsOk)
        {
            return Result<Unit, Err>.Err(_totalFollow.Error);
        }

        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> Follows(int userId, int userFollowingId)
    {
        var follows = await _userRepository.Follows(userId, userFollowingId);
        if (!follows.IsOk)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }
        var updateFollow = await UpdateTotalFollows(userId,userFollowingId);
        if (!updateFollow.IsOk)
        {
            return Result<Unit, Err>.Err(updateFollow.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }
}