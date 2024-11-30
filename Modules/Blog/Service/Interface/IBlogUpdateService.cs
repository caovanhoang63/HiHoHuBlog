using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface IBlogUpdateService
{
    Task<Result<Unit,Err>> UpdateTitle(IRequester requester,int id ,string? title);
    Task<Result<Unit,Err>> UpdateContent(IRequester requester,int id, string? content);
    Task<Result<Unit,Err>> UpdateThumbnail(IRequester requester,int id, Image? image);
    Task<Result<Unit,Err>> Publish(IRequester requester,int id , string content);
    Task<Result<Unit,Err>> UnPublish(IRequester requester,int id);
    Task<Result<Unit,Err>> UpdateTotalLikes(int id);
    Task <Result<int?,Err>> GetTotalLikes(int blogId);
    
    
    Task<Result<Unit,Err>> LikeBlog(int userId,int blogId);
    Task<Result<Unit,Err>> DisikeBlog(int userId,int blogId);
    Task<Result<bool,Err>> IsLiked(int userId,int blogId);
    
    
    Task<Result<Unit,Err>> UpdateTotalComments(int id);
    
    Task<Result<Unit,Err>> Comments(int userId,int blogId,string content);
    
    Task<Result<IEnumerable<UserCommentBlog>?, Err>> GetCommentsById(int blogId);    

}