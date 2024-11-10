using Amazon.S3;
using Amazon.S3.Model;

namespace HiHoHuBlog.Utils;

public class S3UploadProvider : IUploadProvider
{
    private readonly AmazonS3Client _s3Client;    
    private readonly string _bucketName;
    private readonly string _cdnUrl;
    public S3UploadProvider(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = (AmazonS3Client)s3Client;
        _bucketName = configuration["AWS:BucketName"];
        _cdnUrl = configuration["AWS:CdnUrl"];
        Console.WriteLine($"Bucket: {_bucketName}");
        Console.WriteLine($"CDN URL: {_cdnUrl}");
    }

    public async Task<Result<Image, Err>> UploadImage(byte[] data, string dst)
    {
        try
        {
            if (string.IsNullOrEmpty(dst))
            {
                return Result<Image, Err>.Err(UtilErrors.InternalServerError(null));
            }

            // Clean the path - remove leading slash and convert backslashes
            dst = dst.TrimStart('/').Replace('\\', '/');

            // Get image metadata
            using var imageStream = new MemoryStream(data);
            using var image = SixLabors.ImageSharp.Image.Load(imageStream);
            
            // Prepare upload request
            using var ms = new MemoryStream(data);
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = dst, // Use the provided path directly
                InputStream = ms,
                ContentType = GetContentType(Path.GetExtension(dst))
            };

            // Add metadata
            putRequest.Metadata.Add("width", image.Width.ToString());
            putRequest.Metadata.Add("height", image.Height.ToString());

            // Upload to S3
            var response = await _s3Client.PutObjectAsync(putRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var imageResult = new Image
                {
                    Id = Path.GetFileName(dst),
                    Url = $"{_cdnUrl}/{dst}",
                    Width = image.Width.ToString(),
                    Height = image.Height.ToString(),
                    CloudName = "AWS",
                    Extension = Path.GetExtension(dst)
                };

                return Result<Image, Err>.Ok(imageResult);
            }

            return Result<Image, Err>.Err(UtilErrors.InternalServerError(null));

        }
        catch (AmazonS3Exception awsEx)
        {
            return Result<Image, Err>.Err(UtilErrors.InternalServerError(null));

        }
        catch (Exception ex)
        {
            return Result<Image, Err>.Err(UtilErrors.InternalServerError(null));

        }
    }

    private string GetContentType(string extension)
    {
        return extension.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}
