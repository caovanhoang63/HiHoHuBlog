namespace HiHoHuBlog.Utils;

public static class RandomString
{
    
    public static async Task<String> RandomString5()
    {
        string result="";
        Random random = new Random();
        while (result.Length < 5)
        {
            if (random.Next(0, 1) == 1)
            {
                result= result + Convert.ToChar(random.Next(65, 90));
            }
            else
            {
                result= result + Convert.ToChar(random.Next(97, 122));
            }
        }
        return result;
    }
}