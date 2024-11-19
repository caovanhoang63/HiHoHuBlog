namespace HiHoHuBlog.Utils;

public class Paging
{
    public Paging(int page,int pageSize)
    {
        PageSize = pageSize;
        Page = page;
        InitDefault();
    }

    public int Page { get; set; }
    public int? Cursor { get; set; }
    public int? NextCursor { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set;  }
    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);

    public int GetOffSet()
    {
        return PageSize * (Page - 1);
    }

    public bool IsLastPage()
    {
        return Page  == (Total / PageSize) + 1;
    }
    
    public void InitDefault()
    {
        if (Page <= 0)
        {
            Page = 1;
        }

        if (PageSize <= 0)
        {
            PageSize = 20;
        }
    }
}