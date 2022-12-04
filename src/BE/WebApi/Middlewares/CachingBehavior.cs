using Digitime.Server.Application.Abstractions;
using EasyCaching.Core;
using MediatR;

namespace Digitime.Server.Middlewares;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
{
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly int _defaultCacheExpirationInHours = 1;

    public CachingBehavior(
        IEasyCachingProviderFactory cachingFactory,
        ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _cachingProvider = cachingFactory.GetCachingProvider("memory");
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheableRequest cacheRequest)
        {
            // Continue to next middleware
            return await next();
        }

        var cacheKey = cacheRequest.GetCacheKey();
        var cachedResponse = await _cachingProvider.GetAsync<TResponse>(cacheKey);
        if (cachedResponse.Value != null)
        {
            _logger.LogDebug($"Fetch data from cache with cacheKey: {cacheKey}");
            return cachedResponse.Value;
        }

        var response = await next();

        if (response is null)
        {
            _logger.LogDebug($"Can't cache the data of {cacheKey} because the value is null.");
            return response;
        }
        
        var expirationTime = cacheRequest.GetCacheExpiration() ?? DateTime.UtcNow.AddHours(_defaultCacheExpirationInHours);
        await _cachingProvider.SetAsync(cacheKey, response, expirationTime.TimeOfDay);
        _logger.LogDebug($"Set data to cache with cacheKey: {cacheKey}");

        return response;
    }
}
