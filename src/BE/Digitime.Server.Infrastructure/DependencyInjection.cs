﻿using Digitime.Server.Application.Abstractions;
using Digitime.Server.Infrastructure.Http;
using Digitime.Server.Infrastructure.Http.Clients;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Server.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Digitime.Server.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddHttpClient()
            .AddScoped<IObtainPublicHolidays, PublicHolidaysClient>()
            .AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value)
            .AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>))
            .AddSingleton<ITimesheetRepository, TimesheetRepository>()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IEmailRepository, EmailRepository>()
            .AddSingleton<IProjectRepository, ProjectRepository>()
            .AddSingleton<IWorkspaceRepository, WorkspaceRepository>()
            .AddSingleton<IPushRepository, FakePushRepository>()
            .AddSingleton<INotificationRepository, NotificationRepository>()
            .AddSingleton<Auth0ManagementClient>();

        services.AddScoped<Auth0TokenHandler>();
        services.AddHttpClient();

        return services;
    }
}
