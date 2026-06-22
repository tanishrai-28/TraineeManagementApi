using TraineeManagementApi.Context;
using TraineeManagementApi.Helpers;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Username = "admin",
                    Email = "admin@test.com",
                    PasswordHash = PasswordHasher.HashPassword("Admin@123"),
                    Role = RoleType.Admin,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
            }
        }
    }
}