namespace HiHoHuBlog.Modules.Admin.Entity;

public class BlogBlocked
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public int ReasonBlogBlockedId {get; set;}
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}