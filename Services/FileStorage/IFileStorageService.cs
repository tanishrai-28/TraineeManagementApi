namespace TraineeManagementApi.Services.FileStorage;

public interface IFileStorageService
{
    Task<string> SaveAsync(Stream stream, string extension, CancellationToken cancellationToken = default);

    Task<Stream> OpenReadAsync(string storageName, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string storageName);

    Task DeleteAsync(string storageName);
}