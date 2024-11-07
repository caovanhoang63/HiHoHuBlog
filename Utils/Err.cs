namespace HiHoHuBlog.Utils;

public class Err : Exception
{
    public int ErrorCode { get; }
    public DateTime Timestamp { get; }
    public Exception InnerError { get; }

    public Err(string? message, int errorCode = 0)
        : base(message)
    {
        ErrorCode = errorCode;
        Timestamp = DateTime.Now;
    }

    public Err(string message, Exception innerException, int errorCode = 0)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Timestamp = DateTime.Now;
        InnerError = innerException;
    }

    public override string ToString()
    {
        var innerErrorMessage = InnerError != null ? $" Inner Exception: {InnerError.Message}" : string.Empty;
        return $"Error {ErrorCode}: {Message} (Occurred at {Timestamp}){innerErrorMessage}";
    }
}

