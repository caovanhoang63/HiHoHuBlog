using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;
using Unit = System.Reactive.Unit;

namespace HiHoHuBlog.Modules.Blog.Repository;

public interface IBlogTagRepository
{
    Task<Result<Unit,Err>> Create(BlogTagCreate b);
    Task<Result<Unit,Err>> DeleteBlogTag(int blogId,int tagId);
}