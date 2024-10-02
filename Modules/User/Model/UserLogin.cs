namespace HiHoHuBlog.Modules.User.Model;

public record UserLogin()
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}