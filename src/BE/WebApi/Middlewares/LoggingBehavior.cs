using System.Diagnostics;
using MediatR;
using Newtonsoft.Json;

namespace Digitime.Server.Middlewares;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request?.GetType().Name;
        var requestGuid = Guid.NewGuid().ToString();

        var requestNameWithGuid = $"{requestName} [{requestGuid}]";

        _logger.LogDebug($"[START] {requestNameWithGuid}");
        TResponse response;

        var stopwatch = Stopwatch.StartNew();
        try
        {
            try
            {
                _logger.LogDebug($"[PROPS] {requestNameWithGuid} {JsonConvert.SerializeObject(request)}");
            }
            catch (NotSupportedException)
            {
                _logger.LogDebug($"[Serialization ERROR] {requestNameWithGuid} Could not serialize the request.");
            }

            response = await next();
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogDebug($"[END] {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
        }

        return response;
    }
}