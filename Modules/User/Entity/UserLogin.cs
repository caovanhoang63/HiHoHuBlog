using System.ComponentModel.DataAnnotations.Schema;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;

public class UserLogin :BaseEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
}