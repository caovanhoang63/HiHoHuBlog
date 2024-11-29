namespace HiHoHuBlog.Modules.Blog.Entity;

using System.ComponentModel.DataAnnotations.Schema;
using HiHoHuBlog.Utils;
using Newtonsoft.Json;
using ThirdParty.Json.LitJson;


[Table("comments")]
public class UserCommentBlog : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BlogId { get; set; }
    public string? Content { get; set; }
    
    public User.Entity.User? User { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}