using HiHoHuBlog.Utils;

namespace HiHoHuBlog.Modules.Tag.Entity;

public class Tag : BaseEntity 
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public int TotalBlog { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}