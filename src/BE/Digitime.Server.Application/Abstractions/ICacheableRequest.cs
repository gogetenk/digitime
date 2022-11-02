using System;

namespace Digitime.Server.Application.Abstractions;
public interface ICacheableRequest
{
    string GetCacheKey();
    DateTime? GetCacheExpiration();
}
