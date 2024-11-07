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
}