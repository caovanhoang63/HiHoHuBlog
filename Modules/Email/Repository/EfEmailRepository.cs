using AutoMapper;
using HiHoHuBlog.Modules.Email.Entity;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog.Modules.Email.Repository;

public class EfEmailRepository(ApplicationDbContext context, IMapper mapper) : IEmailRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly DbSet<EmailTemplate> _dbSet = context.Set<EmailTemplate>();

    public async Task<Result<Unit, Err>> Create(EmailTemplateCreate c)
    {
        try
        {
             await _dbSet.AddAsync(_mapper.Map<EmailTemplate>(c));
             await _context.SaveChangesAsync();
             return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<EmailTemplate?, Err>> FindByName(string name)
    {
        try
        {
            var r =  await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
            return Result<EmailTemplate?, Err>.Ok(r);
        }
        catch (Exception ex)
        {
            return Result<EmailTemplate?, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<EmailTemplate?, Err>> FindById(int id)
    {
        try
        {
             var r =  await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            return Result<EmailTemplate?, Err>.Ok(r);
        }
        catch (Exception ex)
        {
            return Result<EmailTemplate?, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<IEnumerable<EmailTemplate>?, Err>> FindMany(string arg, Paging paging)
    {
        try
        {
            
            var r =  await _dbSet
                .Where(e => e.Name.StartsWith(arg))
                .OrderByDescending(x => x.Id).Take(paging.PageSize).Skip(paging.GetOffSet()).ToListAsync();
            
            return Result<IEnumerable<EmailTemplate>?, Err>.Ok(r);

        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EmailTemplate>?, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> Update(int id, EmailTemplateUpdate u)
    {
        try
        {
            await _dbSet.Where(e => e.Id == id)
                .ExecuteUpdateAsync(ee => ee
                    .SetProperty(p => p.Name, u.Name)
                    .SetProperty(p=> p.Content, u.Content));
            
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }

    public async Task<Result<Unit, Err>> Delete(int id)
    {
        try
        {
            await _dbSet.Where(e => e.Id == id)
                .ExecuteUpdateAsync(ee => ee
                    .SetProperty(p=> p.Status, (int)Status.Deleted));
            return Result<Unit, Err>.Ok(new Unit());
        }
        catch (Exception ex)
        {
            return Result<Unit, Err>.Err(UtilErrors.InternalServerError(ex));
        }
    }
}