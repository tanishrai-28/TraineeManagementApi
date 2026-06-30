using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace TraineeManagementApi.Services.Redis;

public class RedisService: IRedisService
{
    private readonly IConnectionMultiplexer _cache;
    private readonly ILogger<RedisService> _logger;

    public RedisService (IConnectionMultiplexer cache, ILogger<RedisService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> IsConnectionAsync()
    {
        try
        {
            const string key = "redis-connection-check";

            await _cache.GetDatabase().StringSetAsync(key, "connected", TimeSpan.FromSeconds(30));

            var value = await _cache.GetDatabase().StringGetAsync(key);

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