using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;

public class UserProfile
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? Address { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public Image? Avatar { get; set; }   // JSON type mapped to JsonDocument in C#
    public int TotalFollower { get; set; } = 0;
    public int TotalFollowing { get; set; } = 0;
    public int TotalMark { get; set; } = 0;
    public int TotalComment { get; set; } = 0;
    public int TotalBlog { get; set; } = 0;
    public int TotalBlogView { get; set; } = 0;
}