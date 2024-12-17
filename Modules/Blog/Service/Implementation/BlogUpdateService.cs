using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Utils;
using HtmlAgilityPack;
using Nest;
using Err = HiHoHuBlog.Utils.Err;

namespace HiHoHuBlog.Modules.Blog.Service.Implementation;

public class BlogUpdateService(IBlogRepository blogRepo) : IBlogUpdateService
{
    private readonly IBlogRepository _blogRepo = blogRepo;
    private IBlogUpdateService _blogUpdateServiceImplementation;

    private async Task<Result<Entity.Blog?,Err>> UpdateValidate(IRequester requester,int id )
    {
        var old = await blogRepo.GetBlogById(id);

        if (!old.IsOk)
        {
            return Result<Entity.Blog?, Err>.Err(old.Error);
        }

        if (old.Value is null || old.Value.Status == 0 )
        {
            return Result<Entity.Blog?, Err>.Err(UtilErrors.ErrEntityNotFound(nameof(Blog)));
        }

        if (old.Value.UserId != requester.GetId())
        {
            return Result<Entity.Blog?, Err>.Err(UtilErrors.ErrNoPermission());
        }

        return old;
    }
    public async Task<Result<Unit, Err>> UpdateTitle(IRequester requester, int id, string title)
    {
        var old = await UpdateValidate(requester, id);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }
        
        
    
        // no need to update
        if (old.Value?.Title == title)
        {
            return Result<Unit, Err>.Ok(new Unit());
        }
        
        var result = await blogRepo.UpdateTitle(id, title);
        
        return !result.IsOk ? result : Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> UpdateContent(IRequester requester, int id, string? content)
    {
        var old = await UpdateValidate(requester, id);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }
        var result = await blogRepo.UpdateContent(id, content);
        return !result.IsOk ? result : Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> UpdateThumbnail(IRequester requester, int id, Image? image)
    {
        var old = await UpdateValidate(requester, id);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }
        
        var r = await _blogRepo.UpdateThumbnail(id, image);
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> Publish(IRequester requester, int id, string content)
    {
        var old = await UpdateValidate(requester, id);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }

        var minToRead = content.Length / 300;

        if (minToRead < 1)
            minToRead = 1;
        
        // get first image
        Image thumbnail = await GetThumbnailFromHtml(content);
        
        var r = await _blogRepo.Publish(id,minToRead, thumbnail);
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }
    private async Task<Image> GetThumbnailFromHtml(string htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var imgNode = doc.DocumentNode.SelectSingleNode("//img");
        if (imgNode != null)
        {
            var thumbnailUrl = imgNode.GetAttributeValue("src", null);
            if (!string.IsNullOrEmpty(thumbnailUrl))
            {
                return new Image
                {
                    Url = thumbnailUrl,
                    // Set other Image properties as needed
                };
            }
        }

        return null;
    }
    public async Task<Result<Unit, Err>> UnPublish(IRequester requester, int id)
    {
        var old = await UpdateValidate(requester, id);
        if (!old.IsOk)
        {
            return Result<Unit, Err>.Err(old.Error);
        }
        
        var r = await _blogRepo.UnPublish(id);
        
        if (!r.IsOk)
        {
            return Result<Unit, Err>.Err(r.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> UpdateTotalLikes(int id)
    {
        var _totalLikes = await _blogRepo.UpdateTotalLikes(id);
        if (!_totalLikes.IsOk)
        {
            return Result<Unit, Err>.Err(_totalLikes.Error);
        }

        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<int?, Err>> GetTotalLikes(int blogId)
    {
        var totalLikes = await _blogRepo.GetTotalLikes(blogId);
        if (!totalLikes.IsOk)
        {
            return Result<int?, Err>.Err(totalLikes.Error);
        }
        return Result<int?, Err>.Ok(totalLikes.Value);
    }

    public async Task<Result<Unit, Err>> LikeBlog(int userId,int blogId)
    {
        var likeBLog = await _blogRepo.LikeBlog(userId, blogId);
        if (!likeBLog.IsOk)
        {
            return Result<Unit, Err>.Err(likeBLog.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> DisikeBlog(int userId, int blogId)
    {
        var disLikeBLog = await _blogRepo.DislikeBlog(userId, blogId);
        if (!disLikeBLog.IsOk)
        {
            return Result<Unit, Err>.Err(disLikeBLog.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<bool, Err>> IsLiked(int userId, int blogId)
    {
        var isLiked = await _blogRepo.IsLiked(userId, blogId);
        if (!isLiked.IsOk)
        {
            return Result<bool, Err>.Err(isLiked.Error);
        }

        return Result<bool,Err>.Ok(isLiked.Value);
    }

    public async Task<Result<Unit, Err>> UpdateTotalComments(int id)
    {
        var _totalComments = await _blogRepo.UpdateTotalComments(id);
        if (!_totalComments.IsOk)
        {
            return Result<Unit, Err>.Err(_totalComments.Error);
        }

        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<Unit, Err>> Comments(int userId, int blogId,string content)
    {
        var comment = await _blogRepo.Comment(userId, blogId,content);
        if (!comment.IsOk)
        {
            return Result<Unit, Err>.Err(comment.Error);
        }
        /*var updateTotalComments = await UpdateTotalComments(blogId);
        if (!updateTotalComments.IsOk)
        {
            return Result<Unit, Err>.Err(updateTotalComments.Error);
        }*/
        return Result<Unit, Err>.Ok(new Unit());
    }

    public async Task<Result<IEnumerable<UserCommentBlog>?, Err>> GetCommentsById(int blogId)
    {
        var listComments = await blogRepo.GetCommentsById(blogId);
        if (!listComments.IsOk)
        {
            return Result<IEnumerable<UserCommentBlog>?, Err>.Err(listComments.Error);
        }
        return Result<IEnumerable<UserCommentBlog>?, Err>.Ok(listComments.Value);
    }
}