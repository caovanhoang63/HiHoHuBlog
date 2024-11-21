namespace HiHoHuBlog.Modules.Blog.Entity;

public class UserReadBlog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BlogId { get; set; }
    
    public virtual Blog? Blog { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}