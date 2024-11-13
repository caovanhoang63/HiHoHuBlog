namespace HiHoHuBlog.Modules.Admin.Entity;

public class ReasonBlogBlock
{
    public int Id { get; set; }
    public string? Message {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}