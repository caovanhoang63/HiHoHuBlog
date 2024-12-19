using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;

[Table("users")]
public class User : BaseEntity
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? FbId { get; set; }
    public string? GgId { get; set; }
    public string? Address { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? Phone { get; set; }
    
    public virtual ICollection<UserLikeBlog>? UserLikeBlogs { get; set; }

    public virtual UserDetails? UserDetails { get; set; }
    public string Role { get; set; } = "user";  // Enum type in MySQL can be represented as string in C#
    public Image? Avatar { get; set; }   // JSON type mapped to JsonDocument in C#
    public int Status { get; set; } = 1;
    public int TotalFollower { get; set; } = 0;
    public int TotalFollowing { get; set; } = 0;
    public int TotalMark { get; set; } = 0;
    public int TotalComment { get; set; } = 0;
    public int TotalBlog { get; set; } = 0;
    public int TotalBlogView { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}