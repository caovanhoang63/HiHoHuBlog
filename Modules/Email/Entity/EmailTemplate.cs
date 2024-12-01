namespace HiHoHuBlog.Modules.Email.Entity;

public class EmailTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public int Status { get; set; } = 1;
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}