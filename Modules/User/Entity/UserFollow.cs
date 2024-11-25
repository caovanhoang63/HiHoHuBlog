using System.ComponentModel.DataAnnotations.Schema;
using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;


[Table("user_follow")]
public class UserFollow : BaseEntity
{
    public int UserId { get; set; }
    public int UserFollowing{ get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}


/*`user_id` INT,
`user_following` INT,
`created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
PRIMARY KEY (`user_id`,`user_following`)*/