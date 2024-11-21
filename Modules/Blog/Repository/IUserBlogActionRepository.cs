using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Repository;

public interface IUserBlogActionRepository
{
    Task<Result<Unit,Err>> ReadBlog(int blogId,int userId);
    Task<Result<IEnumerable<UserReadBlogList>?, Err>>ListReadHistory(int userId, Paging paging);
}