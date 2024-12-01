namespace HiHoHuBlog.Modules.Email.Entity;

public class MailRequest
{
    public List<string> toAddresses;
    public List<string> ccAddresses;
    public List<string> bccAddresses;
    public string bodyHtml;
    public string bodyText;
    public string subject;
    public string senderAddress;
}