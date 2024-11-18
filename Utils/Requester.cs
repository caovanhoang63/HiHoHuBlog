namespace HiHoHuBlog.Utils;

public interface IRequester
{
    int GetId();
    string GetSystemRole();
    string GetUsername();
}


public class Requester : IRequester
{
    private string id, role, username;

    public Requester(string id, string role,string username)
    {
        this.id = id;
        this.role = role;
        this.username = username;
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
}