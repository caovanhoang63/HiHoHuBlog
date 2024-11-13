namespace HiHoHuBlog.Modules.Blog.Entity;

public class BlogList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public UserDto? User { get; set; }

    public int TotalView { get; set; } = 0;
    public int TotalMark { get; set; } = 0;
    public int TotalLike { get; set; } = 0;
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}