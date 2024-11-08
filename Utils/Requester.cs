namespace HiHoHuBlog.Utils;

public interface IRequester
{
    int GetId();
    string GetSystemRole();
}


public class Requester : IRequester
{
    public int GetId()
    {
        return 0;
    }

    public string GetSystemRole()
    {
        return "user";
    }
}