using HiHoHuBlog.Modules.Tag.Entity;
using HiHoHuBlog.Utils;
using Unit = System.Reactive.Unit;

namespace HiHoHuBlog.Modules.Tag.Repository.Interface;

public interface ITagRepository
{
    Task<Result<Unit,Err>> Create(TagCreate tag);
    Task<Result<Unit, Err>> Delete(int id);
    Task<Result<IEnumerable<Entity.Tag>?,Err>> List(TagFilter? tagFilter,Paging paging);
}