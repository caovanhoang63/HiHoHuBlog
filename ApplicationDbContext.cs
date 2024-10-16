using Microsoft.EntityFrameworkCore;

namespace HiHoHuBlog;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
    }   
}