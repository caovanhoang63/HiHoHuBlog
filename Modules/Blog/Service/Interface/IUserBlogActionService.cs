using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface IUserBlogActionService
{
    Task<Result<Unit,Err>> ReadBlog(IRequester requester,int blogId);
    Task<Result<IEnumerable<UserReadBlogList>?, Err>>ListReadHistory(IRequester requester,int userId, Paging paging);
}