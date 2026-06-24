namespace TraineeManagementApi.Services.Redis;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);

    Task SetAsync<T>(string key, T value, int expiry);

    Task RemoveAsync (string key);
}