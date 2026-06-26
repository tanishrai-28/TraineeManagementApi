using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TraineeManagementApi.Services.Redis;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await _cache.GetStringAsync(key);

            if (value == null)
            {
                return default;
            }

            _logger.LogInformation("Data fetched from cache");

            return JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Redis offline. Fallback to MySQL database");
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, int expiry)
    {
        try
        {
            var serializeValue = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(key, serializeValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiry) // Add expiry from input
            });
            _logger.LogInformation("Data added to cache");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to store in cache");
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
            _logger.LogInformation("Data removed from cache");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to delete from cache");
        }
    }
}