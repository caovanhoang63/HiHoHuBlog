using HiHoHuBlog.Modules.Tag.Entity;
using HiHoHuBlog.Utils;
using Unit = System.Reactive.Unit;

namespace HiHoHuBlog.Modules.Tag.Repository.Interface;

public interface ITagRepository
{
    Task<Result<Unit,Err>> Create(TagCreate tag);
    Task<Result<Unit, Err>> Delete(int id);
    Task<Result<IEnumerable<Entity.Tag>?,Err>> ListWithPaging(TagFilter? tagFilter,Paging paging);
    Task<Result<IEnumerable<Entity.Tag>?, Err>> FindWithFilter(TagFilter? tagFilter);
    Task<Result<Unit, Err>> UpdateTotalBlogsForTagsAsync();
}