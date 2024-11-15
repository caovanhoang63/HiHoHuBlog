using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace HiHoHuBlog.Utils;

public static class AuthUtils
{
    public static async Task<IRequester> GetInfo(Task<AuthenticationState> authenticationStateTask)
    {
        var authUser = await authenticationStateTask;
        var user = authUser.User;
        
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        var userEmailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        Requester requester = new Requester(userIdClaim, roleClaim,userEmailClaim);
        
        return requester;
    }
}