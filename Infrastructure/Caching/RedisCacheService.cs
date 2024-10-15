using Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;


namespace Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache; 
    
    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        string? value = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (string.IsNullOrEmpty(value))
            return default;

        return JsonConvert.DeserializeObject<T>(value);
    }

    public async Task SetAsync<T>(
        string key, T value, 
        CancellationToken cancellationToken,
        DistributedCacheEntryOptions? options = null)
    {
        var jsonData = JsonConvert.SerializeObject(value);
        if (options is not null)
        {
            await _distributedCache.SetStringAsync(key, jsonData, options, cancellationToken);
        }
        else
        {
            await _distributedCache.SetStringAsync(key, jsonData, cancellationToken);
        }
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return  _distributedCache.RemoveAsync(key, cancellationToken);
    }
}