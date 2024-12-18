using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;
using Nest;

namespace HiHoHuBlog.Modules.Blog.Repository;

public interface IBlogRepository
{
    Task<Result<Unit,Err>> Create(BlogCreate blog);
    Task<Result<Unit, Err>> UpdateMetaData(BlogMetaDataUpdate blog);
    Task<Result<Unit, Err>> DeleteBlog(int id);
    Task<Result<Unit, Err>> BanBlog(int id);
    Task<Result<Entity.Blog?, Err>> GetBlogById(int id);
    Task<Result<BlogDetail?, Err>> GetBlogDetail(int id);
    Task<Result<Unit,Err>> UpdateTitle(int id ,string title);
    Task<Result<Unit,Err>> UpdateContent(int id ,string? content);
    Task<Result<IEnumerable<BlogList>?, Err>> GetBlogList(BlogFilter? filter,Paging paging);
    Task<Result<IEnumerable<Entity.Blog>?, Err>> ListBlogs(BlogFilter? filter, Paging? paging);
    Task<Result<Unit,Err>> UpdateThumbnail(int id ,Image? thumbnail);
    Task<Result<Unit, Err>> Publish(int id, int minToRead,Image? thumbnail);
    Task<Result<Unit, Err>> UnPublish(int id);
    Task<Result<int?, Err>> GetTotalLikes(int blogId);
    Task<Result<Unit,Err>> UpdateTotalLikes(int id);
    Task<Result<Unit,Err>> LikeBlog(int userId,int blogId);
    Task<Result<Unit,Err>> DislikeBlog(int userId,int blogId);
    Task<Result<bool,Err>> IsLiked(int userId,int blogId); 
    Task<Result<Unit,Err>> UpdateTotalBookmarks(int blogId);

    Task<Result<Unit,Err>> BookmarkBlog(int userId,int blogId);
    Task<Result<Unit,Err>> UnBookmarkBlog(int userId,int blogId);
    Task<Result<bool,Err>> IsBookmarked(int userId,int blogId);
    
    Task<Result<Unit,Err>> UpdateTotalComments(int id);
    Task<Result<Unit,Err>> Comment(int userId,int blogId,string content);
    Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogListProfile(BlogFilter? filter,Paging paging);
    Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogListFavorite(BlogFilter? filter,Paging paging);
    Task<Result<IEnumerable<BlogListProfile>?, Err>> GetBlogListBookmark(BlogFilter? filter,Paging paging);

    Task<Result<IEnumerable<UserCommentBlog>?,Err>> GetCommentsById(int blogId);


}