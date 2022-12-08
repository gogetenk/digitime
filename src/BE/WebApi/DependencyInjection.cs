using System.Reflection;
using Digitime.Server.Middlewares;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Digitime.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddSingleton(config)
            .AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
