using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Configurations;
using TraineeManagementApi.Context;
using TraineeManagementApi.Services;
using TraineeManagementApi.Services.FileStorage;
using TraineeManagementApi.Services.Interface;
using TraineeManagementApi.Services.RabbitMq;
using TraineeManagementApi.Services.Redis;

namespace TraineeManagementApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStaticServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITraineeService, TraineeService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMentorService, MentorService>();
        services.AddScoped<ILearningTaskService, LearningTaskService>();
        services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<ISubmissionFileService, SubmissionFileService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        services.AddScoped<IRedisService, RedisService>();

        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySQL")!;

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(connectionString);
        });

        return services;
    }

    public static IServiceCollection AddRedisContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "TraineeManagement:";
        });

        return services;
    }

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins(configuration["CORS-Origins:React"]!).AllowAnyMethod().AllowAnyHeader();
                }
                );
        });

        return services;
    }
}