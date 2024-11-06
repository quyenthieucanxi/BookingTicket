using Microsoft.Extensions.Caching.Distributed;

namespace Application.Abstractions.Messaging;

public interface ICacheAside
{
    Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

}