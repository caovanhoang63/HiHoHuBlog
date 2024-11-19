namespace HiHoHuBlog.Modules.Search.Entity;

public class TagSearchDoc
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TotalBlog { get; set; }
    public int Status { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}