using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Entity;

public class BlogCreate : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string Slug { get; set; }
}