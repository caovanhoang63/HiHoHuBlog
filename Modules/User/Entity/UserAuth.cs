using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;

public class UserAuth :BaseEntity
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Username { get; set; }
}