using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HiHoHuBlog.Modules.Admin.Entity;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.Email.Entity;
using HiHoHuBlog.Modules.Tag.Entity;
using HiHoHuBlog.Modules.User.Entity;
using HiHoHuBlog.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using User = HiHoHuBlog.Modules.User.Entity.User;

namespace HiHoHuBlog;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){}
    
    public DbSet<UserLikeBlog> UserLikeBlogs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserDetails> UserDetails { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BlogTag> BlogTags { get; set; }
    public DbSet<BlogBlocked> BlogBlocked { get; set; }
    public DbSet<ReasonBlogBlock> ReasonBlogBlock { get; set; }
    public DbSet<UserCommentBlog> UserCommentBlogs { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }
    public DbSet<UserReadBlog> userReadBlog { get; set; }
    public DbSet<EmailTemplate> emailTemplate { get; set; }
    public DbSet<UserBookmarkBlog> UserBookmarkBlogs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Blog>().ToTable("blogs");
        modelBuilder.Entity<Tag>().ToTable("tags");
        modelBuilder.Entity<BlogTag>().ToTable("blog_tag");
        modelBuilder.Entity<BlogTag>().HasKey(b => b.TagId);
        modelBuilder.Entity<BlogTag>().HasKey(b => b.BlogId);
        modelBuilder.Entity<UserDetails>().ToTable("user_details");
        modelBuilder.Entity<BlogBlocked>().ToTable("blog_blocked");
        modelBuilder.Entity<ReasonBlogBlock>().ToTable("reason_blog_block");
        modelBuilder.Entity<UserLikeBlog>().ToTable("user_like_blog");
        modelBuilder.Entity<UserFollow>().ToTable("user_follow");
        modelBuilder.Entity<UserCommentBlog>().ToTable("comments");


        modelBuilder.Entity<UserReadBlog>().ToTable("user_read_blogs");
        modelBuilder.Entity<EmailTemplate>().ToTable("email_template");
        modelBuilder.Entity<UserBookmarkBlog>().ToTable("bookmarks");
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserDetails)       
            .WithOne()                        
            .HasForeignKey<UserDetails>(ud => ud.UserId);
        modelBuilder.Entity<Blog>()
            .Property(b => b.Thumbnail)
            .HasConversion<string>(
                v => JsonConvert.SerializeObject(v),       
                v => JsonConvert.DeserializeObject<Image?>(v) 
            );
        
        modelBuilder.Entity<BlogTag>()
            .HasKey(bt => new { bt.BlogId, bt.TagId }); // Khóa chính của BlogTag là BlogId và TagId
        
        modelBuilder.Entity<Blog>().HasMany(e=>e.Tags).WithMany(t => t.Blogs).UsingEntity<BlogTag>();;
        
        modelBuilder.Entity<User>()
            .Property(b => b.Avatar)
            .HasConversion<string>(
                v => JsonConvert.SerializeObject(v),       
                v => JsonConvert.DeserializeObject<Image?>(v) 
            );
        modelBuilder.Entity<UserLikeBlog>()
            .HasKey(ulb=> new {ulb.BlogId, ulb.UserId});
        modelBuilder.Entity<UserCommentBlog>()
            .HasKey(ulb => new {ulb.Id});
        modelBuilder.Entity<UserFollow>()
            .HasKey(ulb=> new {ulb.UserId, ulb.UserFollowing});
        modelBuilder.Entity<UserLikeBlog>()
            .HasOne(ulb => ulb.User)
            .WithMany(u => u.UserLikeBlogs)
            .HasForeignKey(ulb => ulb.UserId);

        modelBuilder.Entity<UserLikeBlog>()
            .HasOne(ulb => ulb.Blog)
            .WithMany(b => b.UserLikeBlogs)
            .HasForeignKey(ulb => ulb.BlogId);
        modelBuilder.Entity<UserBookmarkBlog>()
            .HasKey(ulb=> new {ulb.UserId, ulb.BlogId});
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                var snakeCaseName = ConvertToSnakeCase(property.Name);
                property.SetColumnName(snakeCaseName);
            }
        }
    }
    
    private static string JsonDocumentToString(JsonDocument document)
    {
        using (var stream = new MemoryStream())
        {
            var writer = new Utf8JsonWriter(stream, new JsonWriterOptions {Indented = true});
            document.WriteTo(writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
    
    private string ConvertToSnakeCase(string name)
    {
        return string.Concat(
                name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
            .ToLower();
    }
    
    
}