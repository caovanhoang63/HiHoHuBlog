using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Admin.Service.Interface;

public interface IReasonBlogBlockService
{
    Task<Result<Unit,Err>> Create(IRequester requester ,ReasonBlogBlockCreate reasonBlogBlock);
    Task<Result<IEnumerable<ReasonBlogBlock>, Err>> ListAll();
}