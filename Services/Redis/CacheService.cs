using System.Text.Json;
using StackExchange.Redis;

namespace TraineeManagementApi.Services.Redis;

public class CacheService : ICacheService
{
    // private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IConnectionMultiplexer cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            if (_cache.IsConnected)
            {
                var value = await _cache.GetDatabase().StringGetAsync(key);
                if (!value.HasValue)
                {
                    return default;
                }

                _logger.LogInformation("Data fetched from cache");
                return JsonSerializer.Deserialize<T>(value.ToString());
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Redis offline. Fallback to MySQL database");
            return default;
        }

        return default;
    }

    public async Task SetAsync<T>(string key, T value, int expiry)
    {
        try
        {
            if (_cache.IsConnected)
            {
                var serializeValue = JsonSerializer.Serialize(value);

                await _cache.GetDatabase().StringSetAsync(key, serializeValue, TimeSpan.FromMinutes(expiry));
                _logger.LogInformation("Data added to cache");
            }
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
            if (_cache.IsConnected)
            {
                await _cache.GetDatabase().KeyDeleteAsync(key);
                _logger.LogInformation("Data removed from cache");
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to delete from cache");
        }
    }
}