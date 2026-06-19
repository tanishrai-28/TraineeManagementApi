namespace TraineeManagementApi.Services.FileStorage;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _root;
    public LocalFileStorageService(IConfiguration configuration)
    {
        _root = configuration["Storage:RootPath"]!;

        if (!Directory.Exists(_root))
        {
            Directory.CreateDirectory(_root);
        }
    }

    public async Task<string> SaveAsync(Stream stream, string extension, CancellationToken cancellationToken = default)
    {
        var storageName = $"{Guid.NewGuid()}{extension}";
        var path = Path.Combine(_root, storageName);

        using (var file = File.Create(path))
        {
            await stream.CopyToAsync(file, cancellationToken);
        }

        return storageName;
    }

    public async Task<Stream> OpenReadAsync(string storageName, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(_root, storageName);

        Stream stream = File.OpenRead(path);

        return stream;
    }

    public async Task<bool> ExistsAsync(string storageName)
    {
        var path = Path.Combine(_root, storageName);

        return File.Exists(path);
    }

    public Task DeleteAsync(string storageName)
    {
        var path = Path.Combine(_root, storageName);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }
}