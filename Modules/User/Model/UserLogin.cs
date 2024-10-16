
using System.ComponentModel.DataAnnotations.Schema;

namespace HiHoHuBlog.Modules.User.Model;

[Table("users")]
public class UserLogin
{
    
    public string? Email { get; set; }
    public string? Password { get; set; }
}