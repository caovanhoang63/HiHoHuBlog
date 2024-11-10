namespace HiHoHuBlog.Utils;

public interface IUploadProvider
{
    Task<Result<Image, Err>> UploadImage(byte[] data,string dst);
}