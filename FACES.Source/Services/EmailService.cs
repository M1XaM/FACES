using Microsoft.AspNetCore.Authorization;

using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

using FACES.Repositories;
using FACES.RequestModels;
using FACES.ResponseModels;
using FACES.Models;

public class SendGridSettings
{
    public required string ApiKey { get; set; }
}

public class EmailService : IEmailService
{
    private readonly string _apiKey;
    private readonly IGenericRepository<Client> _clientRepo;

    public EmailService(IOptions<SendGridSettings> options, IGenericRepository<Client> clientRepo)
    {
        _apiKey = options.Value.ApiKey;
        _clientRepo = clientRepo;
    }

    [Authorize]
    public async Task<EmailResponse> SendEmail(EmailRequest emailRequest)
    {
        IEnumerable<Client> clients = await _clientRepo.GetAllAsync();
        var emailTasks = new List<Task>();
        foreach (Client client in clients)
        {
            emailTasks.Add(SendEmailAsync(client.Email, emailRequest.Title, emailRequest.Message));
        }

        await Task.WhenAll(emailTasks);
        return new EmailResponse { Success = true };
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
