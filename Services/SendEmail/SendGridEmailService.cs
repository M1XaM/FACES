using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;

public class SendGridEmailService : IEmailService
{
    private readonly string _apiKey;

    public SendGridEmailService(IOptions<SendGridSettings> options)
    {
        _apiKey = options.Value.ApiKey;
    }

    public async Task SendEmailAsync(string to, string subject, string message)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress("andreiandrei02ipw@gmail.com", "FACES");
        var toAddress = new EmailAddress(to);
        var msg = MailHelper.CreateSingleEmail(from, toAddress, subject, message, message);
        var response = await client.SendEmailAsync(msg);

        
    }
}
