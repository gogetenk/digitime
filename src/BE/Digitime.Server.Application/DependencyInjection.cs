using Digitime.Server.Application.Abstractions;
using Digitime.Server.Application.Notifications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Digitime.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddScoped<INotificationFactory, NotificationFactory>();

        return services;
    }
}
