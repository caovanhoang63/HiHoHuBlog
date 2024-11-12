namespace HiHoHuBlog.Utils;

public class Paging
{
    public Paging(int cursor, int nextCursor, int pageSize, int total)
    {
        Cursor = cursor;
        NextCursor = nextCursor;
        PageSize = pageSize;
        Total = total;
    }

    public int Cursor { get; set; }
    public int NextCursor { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set;  }
}