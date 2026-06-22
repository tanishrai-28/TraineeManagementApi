namespace TraineeManagementApi.Services.Redis;

public interface IRedisService
{
    Task<bool> IsConnectionAsync();
}