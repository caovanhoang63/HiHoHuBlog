using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Search.Entity;

public class UserSearchDoc
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public Image? Avatar { get; set; }
    public UserDetails? UserDetails { get; set; }
    public int Status { get; set; } = 1;
    public int TotalFollower { get; set; } = 0;
    public int TotalMark { get; set; } = 0;
}