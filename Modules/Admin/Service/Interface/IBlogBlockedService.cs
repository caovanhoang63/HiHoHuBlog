using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Admin.Service.Interface;

public interface IBlogBlockedService
{
    Task<Result<Unit,Err>> Create(IRequester requester ,BlogBlockedCreate data);
}