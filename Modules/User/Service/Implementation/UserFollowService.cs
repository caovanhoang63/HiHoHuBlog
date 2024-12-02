using HiHoHuBlog.Modules.User.Repository;
using HiHoHuBlog.Modules.User.Service.Interface;
using HiHoHuBlog.Utils;
using Microsoft.AspNetCore.Http.HttpResults;

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
        if (userFollowingId  == userId) 
            return Result<Unit, Err>.Err(new Err("Can't follow yourself"));

        var user = await _userRepository.FindById( userFollowingId);

        if (user.Value is null)
            return UtilErrors.ErrEntityNotFound("user");
        
        var follows = await _userRepository.Follows(userId, userFollowingId);
        if (!follows.IsOk)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }
        /*var updateFollow = await UpdateTotalFollows(userId,userFollowingId);
        if (!updateFollow.IsOk)
        {
            return Result<Unit, Err>.Err(updateFollow.Error);
        }*/
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> UnFollow(int userId, int userFollowingId)
    {
        var unFollow = await _userRepository.UnFollow(userId, userFollowingId);
        if (!unFollow.IsOk)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }
        /*var updateFollow = await UpdateTotalFollows(userId,userFollowingId);
        if (!updateFollow.IsOk)
        {
            return Result<Unit, Err>.Err(updateFollow.Error);
        }*/
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<bool, Err>> IsFollowed(int userId, int userFollowingId)
    {
        var isFollowed = await _userRepository.IsFollowed(userId, userFollowingId);
        if (!isFollowed.IsOk)
        {
            return Result<bool, Err>.Err(isFollowed.Error);
        }
        return Result<bool, Err>.Ok(isFollowed.Value);
    }
}