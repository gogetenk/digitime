using Digitime.Server.Domain.Ports;
using Digitime.Server.Infrastructure.MongoDb;
using Digitime.Server.Infrastructure.Repositories;
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
            .AddScoped<IObtainPublicHolidays, PublicHolidaysRepository>()
            .AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value)
            .AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>));

        return services;
    }
}
