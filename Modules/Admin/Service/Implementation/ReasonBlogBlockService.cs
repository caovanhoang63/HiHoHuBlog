using AutoMapper;
using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Modules.Admin.Repository.Interface;
using HiHoHuBlog.Modules.Admin.Service.Interface;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog.Modules.Admin.Service.Implementation;

public class ReasonBlogBlockService(IReasonBlogBlockRepository _repo) : IReasonBlogBlockService
{
    public async Task<Result<Unit, Err>> Create(IRequester requester ,ReasonBlogBlockCreate reasonBlogBlock)
    {
        if (requester.GetSystemRole() != "admin" && requester.GetSystemRole() != "mod")
        {
            return Result<Unit,Err>.Err(UtilErrors.ErrNoPermission());
        }

        var r = await _repo.Create(reasonBlogBlock);
        
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        
        return Result<Unit, Err>.Ok(new Unit());
        
    }

    public async Task<Result<IEnumerable<ReasonBlogBlock>, Err>> ListAll()
    {
        var r =  await _repo.ListAll();
        if (!r.IsOk)
        {
            return Result<IEnumerable<ReasonBlogBlock>, Err>.Err(r.Error);
        }

        return r;
    }
}