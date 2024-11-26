using System.ComponentModel.DataAnnotations.Schema;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Blog.Entity;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public virtual UserDetails? UserDetails{ get; set; }
    public string? Address { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Role { get; set; } = "user";  
    public Image? Avatar { get; set; } 
}