using Digitime.Server.Application.Abstractions;
using Digitime.Server.Domain.Notifications;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Digitime.Server.Infrastructure.Http;

public class FakePushRepository : IPushRepository
{
    private readonly ILogger<FakePushRepository> _logger;

    public FakePushRepository(ILogger<FakePushRepository> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(Notification notification)
    {
        _logger.LogInformation("Sending notification " + JsonConvert.SerializeObject(notification));
        return Task.CompletedTask;
    }
}
