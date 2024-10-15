using Microsoft.Extensions.Caching.Distributed;

namespace Application.Abstractions;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken,DistributedCacheEntryOptions? options = null);
    Task RemoveAsync(string key, CancellationToken cancellationToken);
}