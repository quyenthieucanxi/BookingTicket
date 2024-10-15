using System.Collections.Concurrent;
using Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Caching;

public static class CacheAside
{
    private static DistributedCacheEntryOptions Default = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
    };  

    private static readonly ConcurrentDictionary<string, SemaphoreSlim> KeyLocks = new();

    
    public static async Task<T?> GetOrCreateAsync<T>(
        this ICacheService cache,
        string key,
        Func<CancellationToken,Task<T>>factory,
        DistributedCacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        T? value = await cache.GetAsync<T>(key, cancellationToken);
        if (value is not null)
        {
            return value;
        } 
        var keyLock =  KeyLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        var hasLock = await keyLock.WaitAsync(2000,cancellationToken);
       if (!hasLock)
       {
           return default;
       }

       try
       {
           value = await cache.GetAsync<T>(key, cancellationToken);
           if (value is not null)
           {
               return value;
           }
           value = await factory(cancellationToken);
           if (value is null)
           {
               return default;
           }
           await cache.SetAsync(key, value, cancellationToken,options ?? Default);
       }
       finally
       {
           keyLock.Release();
           if (keyLock.CurrentCount == 1)
           {
               KeyLocks.TryRemove(key, out _);
           }
       }
       return value;
        
    }
    
}