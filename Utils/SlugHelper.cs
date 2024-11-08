using System.Text.RegularExpressions;

namespace HiHoHuBlog.Utils;

public static class SlugHelper
{
    private static readonly string[] VietnameseSigns = new string[]
    {
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
    };

    public static string GenerateSlug(string title)
    {
        if (string.IsNullOrEmpty(title)) return "";
        
        var slug = title.ToLower();
        
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
            {
                slug = slug.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
        }

        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        
        slug = Regex.Replace(slug, @"\s+", "-");
        
        slug = Regex.Replace(slug, @"-+", "-");
        
        slug = slug.Trim('-');

        
        return slug;
    }
    
    public static string AppendIdToSlug(string slug, int id)
    {
        return $"{slug}-{id}";
    }
}
