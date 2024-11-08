namespace HiHoHuBlog.Utils;

public static class UtilErrors
{
    public static Err InternalServerError(Exception ex)
    {
        return new Err("Something went wrong in server",ex,500);
    }

    public static Err ErrValidatorError(List<string?> msg)
    {
        return new Err(msg[0],404);
    }

    public static Err ErrEntityNotFound(string entityName)
    {
        return new Err($"{entityName} not found");
    }


    public static Err ErrNoPermission()
    {
        return new Err("You do not have permission to access this resource");
    }
}