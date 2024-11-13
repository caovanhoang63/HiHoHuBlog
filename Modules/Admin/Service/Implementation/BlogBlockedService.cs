using AutoMapper;
using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Modules.Admin.Repository.Interface;
using HiHoHuBlog.Modules.Admin.Service.Interface;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog.Modules.Admin.Service.Implementation;

public class BlogBlockedService(IBlogBlockedRepository _repo, IBlogRepository blogRepo) : IBlogBlockedService
{
    public async Task<Result<Unit, Err>> Create(IRequester requester ,BlogBlockedCreate data)
    {
        if (requester.GetSystemRole() != "admin" && requester.GetSystemRole() != "mod")
        {
            return Result<Unit,Err>.Err(UtilErrors.ErrNoPermission());
        }
        
        var old = await blogRepo.GetBlogById(data.BlogId);

        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }

        if (old.Value is null)
        {
            return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("blog"));
        }

        // Already ban
        if (old.Value.Status == (int)Status.Banned)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }
        
        var bR =  await blogRepo.BanBlog(old.Value.Id);
        if (!bR.IsOk)
        {
            return Result<Unit, Err>.Err(bR.Error);
        }
        
        var r = await _repo.Create(data);
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(bR.Error);
        }
        
        return Result<Unit, Err>.Ok(new Unit());
        
    }
}