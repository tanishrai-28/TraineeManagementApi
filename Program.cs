using RabbitMQ.Client;
using TraineeManagementApi.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TraineeManagementApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfiguration(builder.Configuration);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext(builder.Configuration);

var rabbitMQSection = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddSingleton(sp => new ConnectionFactory()
{
    HostName = rabbitMQSection["HostName"]!,
    Port = Convert.ToInt32(rabbitMQSection["Port"]),
    UserName = rabbitMQSection["UserName"]!,
    Password = rabbitMQSection["Password"]!,
    VirtualHost = rabbitMQSection["VirtualHost"]!
});

builder.Services.AddStaticServices(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);


builder.Services.AddRedisContext(builder.Configuration);

builder.Services.AddHealthChecksExtensions(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.MapHealthChecks(
    "/health/live",
    new HealthCheckOptions
    {
        Predicate = _ => false
    }
);

app.MapHealthChecks(
    "/health/ready",
    new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = HealthCheckReporter.WriteHealthCheckResponse
    }
);

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();


app.Run();
