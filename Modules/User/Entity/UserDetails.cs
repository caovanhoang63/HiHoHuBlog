using System.ComponentModel.DataAnnotations.Schema;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;

[Table("user_details")]
public class UserDetails:BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Bio { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}