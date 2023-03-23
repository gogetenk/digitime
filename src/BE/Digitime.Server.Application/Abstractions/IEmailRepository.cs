using System.Threading.Tasks;

namespace Digitime.Server.Application.Abstractions;

public interface IEmailRepository
{
    Task SendEmailAsync(string recipientEmail, string subject, string content);
}
