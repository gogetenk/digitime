using Digitime.Server.Application.Abstractions;
using DnsClient.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Digitime.Server.Infrastructure.Http;
public class EmailRepository : IEmailRepository
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailRepository> _logger;

    public EmailRepository(IConfiguration configuration, ILogger<EmailRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string content)
    {
        var client = new SendGridClient(_configuration["SendGrid:ApiKey"]);
        var from = new EmailAddress(_configuration["SendGrid:FromEmail"], _configuration["SendGrid:FromName"]);
        var to = new EmailAddress(recipientEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        var response = await client.SendEmailAsync(msg);
        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            throw new ApplicationException("Email sending failed.");
        }
        _logger.LogDebug($"Invitation email sent to {recipientEmail} with success");
    }
}
