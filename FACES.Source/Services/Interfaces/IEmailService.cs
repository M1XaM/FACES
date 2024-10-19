using FACES.RequestModels;
using FACES.ResponseModels;

public interface IEmailService
{
    Task<EmailServiceResponse> SendEmailAsync(EmailViewModel emailRequest);
    Task StartSendingAsync(string to, string subject, string message);
}
