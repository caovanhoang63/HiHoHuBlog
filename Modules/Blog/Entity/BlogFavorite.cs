namespace HiHoHuBlog.Modules.Blog.Entity;

public class BlogFavorite
{
    public int UserId { get; set; }
    public int BlogId { get; set; }
    public BlogListProfile? BlogListProfile { get; set; }
}