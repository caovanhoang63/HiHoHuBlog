using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

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
}