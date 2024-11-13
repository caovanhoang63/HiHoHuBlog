using HiHoHuBlog.Utils;
using Unit = System.Reactive.Unit;

namespace HiHoHuBlog.Modules.Blog.Service.Interface;

public interface IBlogDeleteService
{
    Task<Result<Unit,Err>> Delete(IRequester requester,int id);
    Task<Result<Unit,Err>> Block(IRequester requester,int id);
}