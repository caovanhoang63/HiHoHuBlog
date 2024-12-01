using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Entity;

public class BlogSearchDoc
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Image? Thumbnail  {get; set; }
    public UserDto?  User{ get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? ShortContent { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int MinToRead { get; set; }
    public int TotalView { get; set; } = 0;
    public int TotalMark { get; set; } = 0;
    public int TotalComment { get; set; } = 0;
    public int TotalLike { get; set; } = 0;
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}