using HiHoHuBlog.Modules.Tag.Entity;
using HiHoHuBlog.Modules.Tag.Repository.Implementation;
using HiHoHuBlog.Modules.Tag.Repository.Interface;
using HiHoHuBlog.Modules.Tag.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Tag.Service.Implementation;

public class TagService(ITagRepository tagRepo) : ITagService
{
    private readonly ITagRepository _tagRepo = tagRepo;

    public async Task<Result<Unit, Err>> Create(IRequester requester, TagCreate tag)
    {
        var vR = tag.Validate();
        
        if (!vR.IsOk) return Result<Unit, Err>.Err(vR.Error);
        
        var r =  await _tagRepo.Create(tag);
        
        if (!r.IsOk) return Result<Unit, Err>.Err(r.Error);
        
        return Result<Unit, Err>.Ok(new Unit());
        
    }

    public async Task<Result<Unit, Err>> Delete(IRequester requester, int id)
    {
        if (requester.GetSystemRole() != "admin" || requester.GetSystemRole() != "mod")
            return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        
        var r =  await _tagRepo.Delete(id);
        
        if (!r.IsOk) return Result<Unit, Err>.Err(r.Error);
        
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<IEnumerable<Entity.Tag>?, Err>> List(IRequester requester, TagFilter? tagFilter, Paging paging)
    {
        var r = await _tagRepo.List(tagFilter, paging);
        
        if (!r.IsOk) return Result<IEnumerable<Entity.Tag>?, Err>.Err(r.Error);
        
        return Result<IEnumerable<Entity.Tag>?, Err>.Ok(r.Value);
    }
}