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
    public DbSet<User> Users {get; set;}
    public DbSet<Mentor> Mentors {get; set;}
    public DbSet<LearningTask> LearningTasks {get; set;}
    public DbSet<TaskAssignment> TaskAssignments {get; set;}
    public DbSet<Submission> Submissions {get; set;}
    public DbSet<Review> Reviews {get; set;}
}