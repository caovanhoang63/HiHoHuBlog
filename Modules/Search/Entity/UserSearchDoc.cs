using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Entity;

public class UserSearchDoc
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? Address { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? Phone { get; set; }
    public Image? Avatar { get; set; }
    public int Status { get; set; } = 1;
    public int TotalFollower { get; set; } = 0;
    public int TotalFollowing { get; set; } = 0;
    public int TotalMark { get; set; } = 0;
    public int TotalComment { get; set; } = 0;
    public int TotalBlog { get; set; } = 0;
    public int TotalBlogView { get; set; } = 0;
}