using System.ComponentModel.DataAnnotations.Schema;
using HiHoHuBlog.Utils;
using Newtonsoft.Json;
using ThirdParty.Json.LitJson;

namespace HiHoHuBlog.Modules.Blog.Entity;

[Table("bookmarks")]
public class UserBookmarkBlog : BaseEntity
{
    public int UserId { get; set; }
    public int BlogId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual User.Entity.User? User { get; set; }
    public virtual Blog? Blog { get; set; }
}