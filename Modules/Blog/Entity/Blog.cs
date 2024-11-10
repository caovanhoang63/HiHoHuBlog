using System.ComponentModel.DataAnnotations.Schema;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Entity;

[Table("blogs")]
public class Blog : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsPublished { get; set; }
    public int TotalView { get; set; } = 0;
    public int TotalMark { get; set; } = 0;
    public int TotalLike { get; set; } = 0;
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}