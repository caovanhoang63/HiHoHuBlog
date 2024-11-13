using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Admin.Repository.Interface;

public interface IReasonBlogBlockRepository
{
    Task<Result<Unit,Err>> Create(ReasonBlogBlockCreate reasonBlogBlock);
    Task<Result<IEnumerable<ReasonBlogBlock>, Err>> ListAll();
}