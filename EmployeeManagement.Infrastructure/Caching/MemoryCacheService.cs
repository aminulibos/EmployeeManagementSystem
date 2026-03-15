using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManagement.Infrastructure.Caching;

public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    private readonly IMemoryCache _cache = cache;

    public Task<T?> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var cacheOptions = new MemoryCacheEntryOptions();
        if (expiration.HasValue)
        {
            cacheOptions.AbsoluteExpirationRelativeToNow = expiration.Value;
        }
        else
        {
            cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        }

        _cache.Set(key, value, cacheOptions);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        await RemoveAsync(prefix);
    }

    public Task ClearAllAsync()
    {
        return Task.CompletedTask;
    }
}

