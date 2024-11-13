namespace HiHoHuBlog.Modules.Admin.Entity;

public class BlogBlockedCreate
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public int ReasonBlogBlockedId {get; set;}
    public virtual ReasonBlogBlock? ReasonBlogBlocked { get; set; }
}