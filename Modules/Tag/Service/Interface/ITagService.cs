using HiHoHuBlog.Modules.Tag.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Tag.Service.Interface;

public interface ITagService
{
    Task<Result<Unit,Err>> Create(IRequester requester, TagCreate tag);
    Task<Result<Unit, Err>> Delete(IRequester requester,int id);
    Task<Result<IEnumerable<Entity.Tag>?,Err>> List(IRequester requester,TagFilter? tagFilter,Paging paging);
    Task<Result<Unit, Err>> UpdateTotalBlogsForTagsAsync();
}