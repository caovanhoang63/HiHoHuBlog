using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Blog.Repository;
using HiHoHuBlog.Modules.Blog.Service.Interface;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Service.Implementation;

public class BlogTagService(IBlogTagRepository blogTagRepository ):IBlogTagService
{
    public async Task<Result<Unit, Err>> AddTagBlog(BlogTagCreate blogTagCreate)
    {
        var r = await blogTagRepository.Create(blogTagCreate);
        if (!r.IsOk)
        { 
            return Result<Unit, Err>.Err(r.Error);
        }
        return Result<Unit, Err>.Ok(new Unit());
    }
}