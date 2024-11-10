using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HiHoHuBlog.Modules.Blog.Entity;
using HiHoHuBlog.Modules.User.Entity;
using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<JsonDocument>();
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Blog>().ToTable("blogs");
        
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