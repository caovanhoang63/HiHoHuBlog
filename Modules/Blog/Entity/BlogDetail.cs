using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Entity;

public class BlogDetail
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Image? Thumbnail  {get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public UserDto? User { get; set; }
    public string Slug { get; set; }
    public bool IsPublished { get; set; }
    public int TotalView { get; set; } = 0;
    public int TotalMark { get; set; } = 0;
    public int TotalLike { get; set; } = 0;
    public int TotalComment { get; set; } = 0;

    public int TotalFollowers { get; set; } = 0;
    public int TotalFollowing { get; set; } = 0;
    public int MinToRead { get; set; } = 0;
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}