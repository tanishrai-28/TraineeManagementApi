using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using StackExchange.Redis;
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
        services.AddScoped<IProcessingJobService, ProcessingJobService>();

        services.AddSingleton<IRedisService, RedisService>();

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
        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = configuration.GetConnectionString("Redis");
        //     options.InstanceName = "TraineeManagement:";
        // });

        services.AddSingleton<IConnectionMultiplexer> (sp =>
        {
            var logger = sp.GetRequiredService<ILogger<Program>>();
            var redis = ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = {configuration.GetConnectionString("Redis")!},
                AbortOnConnectFail = false,
                ConnectRetry = 3,
                ReconnectRetryPolicy = new ExponentialRetry(5000)
            });

            redis.ConnectionFailed += (sender, args) =>
                logger.LogError(args.Exception, "Redis Connection failed. Endpoint: {Endpoint}", args.EndPoint);

            redis.ConnectionRestored += (sender, args) =>
                logger.LogInformation(args.Exception, "Redis Connection restored. Endpoint: {Endpoint}", args.EndPoint);

            redis.ErrorMessage += (sender, args) =>
                logger.LogError(args.Message, "Redis Error. Endpoint: {Endpoint}", args.EndPoint);

            redis.ConfigurationChanged += (sender, args) =>
                logger.LogInformation("Redis Connection Configuration changed. Endpoint: {Endpoint}", args.EndPoint);

            return redis;
        });

        return services;
    }

    public static IServiceCollection AddHealthChecksExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddMySql(
                configuration.GetConnectionString("MySQL")!,
                name: "mysql",
                tags: new [] {"ready"}
            )
            .AddRedis(
                configuration.GetConnectionString("Redis")!,
                name: "redis",
                tags: new [] {"ready"}
            )
            .AddRabbitMQ(
                async sp => await sp.GetRequiredService<ConnectionFactory>().CreateConnectionAsync(),
                name: "RabbitMQ",
                tags: new [] {"ready"}
            );


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