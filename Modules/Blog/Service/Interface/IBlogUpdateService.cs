using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface IBlogUpdateService
{
    Task<Result<Unit,Err>> UpdateTitle(IRequester requester,int id ,string? title);
    Task<Result<Unit,Err>> UpdateContent(IRequester requester,int id, string? content);
    Task<Result<Unit,Err>> UpdateThumbnail(IRequester requester,int id, Image? image);
    Task<Result<Unit,Err>> Publish(IRequester requester,int id , string content);
    Task<Result<Unit,Err>> UnPublish(IRequester requester,int id);
}