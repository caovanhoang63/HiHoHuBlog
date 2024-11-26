using HtmlAgilityPack;

namespace HiHoHuBlog.Utils;

public static class GetThumbnail
{
    public static async Task<Image?> GetThumbnailFromHtml(string? htmlContent)
    {
        if (htmlContent != null)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var imgNode = doc.DocumentNode.SelectSingleNode("//img");
            if (imgNode != null)
            {
                var thumbnailUrl = imgNode.GetAttributeValue("src", null);
                if (!string.IsNullOrEmpty(thumbnailUrl))
                {
                    return new Image
                    {
                        Url = thumbnailUrl,
                        // Set other Image properties as needed
                    };
                }
            }
        }
        return null;
    }
}