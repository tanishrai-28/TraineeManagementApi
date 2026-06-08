using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Trainee> Trainees {get; set;}
}