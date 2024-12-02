namespace HiHoHuBlog.Utils;

public interface IRequester
{
    int GetId();
    string GetSystemRole();
    string GetUsername();
    string? GetAvatarUrl();
}


public class Requester : IRequester
{
    private string id, role, username;
    
    private string? avatarUrl;

    public Requester(string id, string role,string username, string? avatarUrl)
    {
        this.id = id;
        this.role = role;
        this.username = username;
        this.avatarUrl = avatarUrl;
    }
    public int GetId()
    {
        return int.Parse(this.id);
    }

    public string GetSystemRole()
    {
        return this.role;
    }

    public string GetUsername()
    {
        return this.username;
    }

    public string? GetAvatarUrl()
    {
        return this.avatarUrl;
    }
}