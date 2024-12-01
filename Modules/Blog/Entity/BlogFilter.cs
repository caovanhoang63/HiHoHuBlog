namespace HiHoHuBlog.Modules.Blog.Entity;

public class BlogFilter
{
    public int[]? Status;
    public int? TagId;
    public DateTime? LtCreatedAt;
    public DateTime? GtCreatedAt;
    public DateTime? LtUpdatedAt;
    public DateTime? GtUpdatedAt;
    public bool? IsPublished;
    public int? UserId;
}