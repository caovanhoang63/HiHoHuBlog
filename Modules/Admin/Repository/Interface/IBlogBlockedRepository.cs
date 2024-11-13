using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Admin.Repository.Interface;

public interface IBlogBlockedRepository
{
    Task<Result<Unit,Err>> Create(BlogBlockedCreate data);
}