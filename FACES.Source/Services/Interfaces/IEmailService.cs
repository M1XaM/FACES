using FACES.RequestModels;
using FACES.ResponseModels;

public interface IEmailService
{
    Task<EmailResponse> SendEmail(EmailRequest emailRequest);
    Task SendEmailAsync(string to, string subject, string message);
}
