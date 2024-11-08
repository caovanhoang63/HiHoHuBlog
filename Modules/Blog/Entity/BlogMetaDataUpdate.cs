using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Entity;

public class BlogMetaDataUpdate: BaseEntity
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string Slug { get; set; }    
}