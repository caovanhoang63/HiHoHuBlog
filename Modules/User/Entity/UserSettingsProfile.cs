using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.User.Entity;

public class UserSettingsProfile : BaseEntity
{
    public int Id { get; set; }
    public Image? Avatar { get; set; }   // JSON type mapped to JsonDocument in C#
    public string LastName { get; set; }= "";
    public string FirstName { get; set; }= "";
    public string ShortBio { get; set; } = "";
}