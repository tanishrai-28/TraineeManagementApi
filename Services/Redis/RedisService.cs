using Microsoft.Extensions.Caching.Distributed;

namespace TraineeManagementApi.Services.Redis;

public class RedisService: IRedisService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisService> _logger;

    public RedisService (IDistributedCache cache, ILogger<RedisService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> IsConnectionAsync()
    {
        try
        {
            const string key = "redis-connection-check";

            await _cache.SetStringAsync(key, "connected", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

            var value = await _cache.GetStringAsync(key);

            if(value == "connected") _logger.LogInformation("Redis online");

            return value == "connected";
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to connect to redis");
            return false;
        }
    }
}