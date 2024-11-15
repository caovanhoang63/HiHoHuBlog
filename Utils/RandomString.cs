namespace HiHoHuBlog.Utils;

public static class RandomString
{
    
    public static  String RandomString5()
    {
        string result="";
        Random random = new Random();
        while (result.Length < 5)
        {
            if (random.Next(0, 2) == 1)
            {
                result= result + Convert.ToChar(random.Next(65, 91));
            }
            else
            {
                result= result + Convert.ToChar(random.Next(97, 123));
            }
        }
        return result;
    }
}