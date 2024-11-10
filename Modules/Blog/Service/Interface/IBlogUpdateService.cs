using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface IBlogUpdateService
{
    Task<Result<Unit,Err>> UpdateTitle(IRequester requester,int id ,string? title);
    Task<Result<Unit,Err>> UpdateContent(IRequester requester,int id, string content);
}