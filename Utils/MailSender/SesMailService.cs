using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Options;

namespace HiHoHuBlog.Utils.MailSender;

public class SesMailService : IMailSender
{
    private readonly IAmazonSimpleEmailService _mailService;
    public SesMailService(IAmazonSimpleEmailService mailService)
    {
        _mailService = mailService;
    }
    
    public async Task<Result<Unit, Err>> Send(MailRequest request)
    {
        
        var messageId = "";
        try
        {
            var response = await _mailService.SendEmailAsync(
                new SendEmailRequest
                {
                    Destination = new Destination
                    {
                        BccAddresses = request.bccAddresses,
                        CcAddresses = request.ccAddresses,
                        ToAddresses = request.toAddresses
                    },
                    Message = new Message
                    {
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = request.bodyHtml
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = request.bodyText
                            }
                        },
                        Subject = new Content
                        {
                            Charset = "UTF-8",
                            Data = request.subject
                        }
                    },
                    Source = request.senderAddress
                });
            messageId = response.MessageId;

        }
        catch (Exception ex)
        {
            Console.WriteLine("SendEmailAsync failed with exception: " + ex.Message);
            return Result<Unit,Err>.Err(UtilErrors.ErrFailToSendEmail(request,ex));
        }
        Console.WriteLine(messageId);
        return Result<Unit, Err>.Ok(new Unit());
    }
}