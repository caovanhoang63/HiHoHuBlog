using HiHoHuBlog.Modules.Email.Entity;
using HiHoHuBlog.Modules.Email.Repository;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Email.Service;

public class EmailService(IEmailRepository emailRepository) : IEmailService
{
    private readonly IEmailRepository _emailRepository = emailRepository;

    private bool Validate(IRequester requester)
    {
        return requester.GetSystemRole() == "admin" || requester.GetSystemRole() == "mod";
    }

    public async Task<Result<Unit, Err>> Create(IRequester requester, EmailTemplateCreate c)
    {
        if (!Validate(requester)) return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        
        var old  = await  _emailRepository.FindByName(c.Name);
        
        if (!old.IsOk) return Result<Unit, Err>.Err(old.Error);

        if (old.Value != null && old.Value.Status == 1 ) return Result<Unit, Err>.Err(UtilErrors.ErrEntityAlreadyExists("Email Template"));
        
        var r = await _emailRepository.Create(c);
        
        if (!r.IsOk) return Result<Unit, Err>.Err(r.Error);
        
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<EmailTemplate?, Err>> FindByName(IRequester requester, string name)
    {
        if (!Validate(requester)) return Result<EmailTemplate?, Err>.Err(UtilErrors.ErrNoPermission());

        var old  = await  _emailRepository.FindByName(name);
        
        
        if (!old.IsOk) return Result<EmailTemplate?, Err>.Err(old.Error);
        
        return Result<EmailTemplate?, Err>.Ok(old.Value);
    }

    public async Task<Result<IEnumerable<EmailTemplate>?, Err>> FindMany(IRequester requester, string arg, Paging paging)
    {
        if (!Validate(requester)) return Result<IEnumerable<EmailTemplate>?, Err>.Err(UtilErrors.ErrNoPermission());

        var old  = await  _emailRepository.FindMany(arg, paging);
        
        if (!old.IsOk) return Result<IEnumerable<EmailTemplate>?, Err>.Err(old.Error);
        
        return Result<IEnumerable<EmailTemplate>?, Err>.Ok(old.Value);
    }

    public async Task<Result<Unit, Err>> Update(IRequester requester, string name , EmailTemplateUpdate u)
    {
        if (!Validate(requester)) return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());
        
        var old  = await  _emailRepository.FindByName(u.Name);
        
        if (!old.IsOk) return Result<Unit, Err>.Err(old.Error);

        if (old.Value is null) return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("Email Template"));
        
        
         var r =  await _emailRepository.Update(old.Value.Id, u);
         
         if (r.IsOk) return Result<Unit, Err>.Ok(new Unit());
        
        return Result<Unit, Err>.Err(r.Error);         
    }

    public async Task<Result<Unit, Err>> Delete(IRequester requester, int id)
    {
        if (!Validate(requester)) return Result<Unit, Err>.Err(UtilErrors.ErrNoPermission());

        var old  = await  _emailRepository.FindById(id);
        
        if (!old.IsOk) return Result<Unit, Err>.Err(old.Error);
        
        if (old.Value == null || old.Value.Status != 1 ) return Result<Unit, Err>.Err(UtilErrors.ErrEntityNotFound("Email Template"));
        
        return await _emailRepository.Delete(id);
    }
}