namespace HiHoHuBlog.Modules.Blog.Entity;

public class BlogTag
{
    public int TagId { get; set; }
    public int BlogId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}