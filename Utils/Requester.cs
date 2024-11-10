namespace HiHoHuBlog.Utils;

public interface IRequester
{
    int GetId();
    string GetSystemRole();
}


public class Requester : IRequester
{
    private string id, role;

    public Requester(string id, string role)
    {
        this.id = id;
        this.role = role;
    }
    public int GetId()
    {
        return 0;
    }

    public string GetSystemRole()
    {
        return "user";
    }
}