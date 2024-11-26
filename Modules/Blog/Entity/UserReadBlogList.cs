namespace HiHoHuBlog.Modules.Blog.Entity;

public class UserReadBlogList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BlogId { get; set; }

    public virtual BlogList? Blog { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}