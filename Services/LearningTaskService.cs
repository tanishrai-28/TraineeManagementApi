using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.LearningTaskDTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Services;

public class LearningTaskService : ILearningTaskService
{
    private readonly ILogger<LearningTaskService> _logger;
    private readonly ApplicationDbContext _context;

    public LearningTaskService(ApplicationDbContext context, ILogger<LearningTaskService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<LearningTaskResponse>> GetAllAsync()
    {
        var learningTasks = await _context.LearningTasks.ToListAsync();

        return learningTasks.Select(MaptoResponse).ToList();
    }

    public async Task<LearningTaskResponse?> GetByIdAsync(long id)
    {
        var learningTask = await _context.LearningTasks.FindAsync(id);

        if (learningTask == null)
        {
            _logger.LogInformation($"Learning Task with {id} not found");
        }

        return learningTask == null ? null : MaptoResponse(learningTask);
    }

    public async Task<LearningTaskResponse> CreateAsync(CreateLearningTaskRequest request)
    {
        var learningTask = new LearningTask()
        {
            Title = request.Title,
            Description = request.Description,
            ExpectedTechStack = request.ExpectedTechStack,
            DueDate = request.DueDate,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _context.LearningTasks.AddAsync(learningTask);

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Learning Task created.");

        return MaptoResponse(learningTask);
    }

    public async Task<bool> UpdateAsync(long id, UpdateLearningTaskRequest request)
    {
        var learningTask = await _context.LearningTasks.FindAsync(id);

        if (learningTask == null) return false;

        learningTask.Title = request.Title;
        learningTask.Description = request.Description;
        learningTask.ExpectedTechStack = request.ExpectedTechStack;
        learningTask.DueDate = request.DueDate;
        learningTask.Status = request.Status;
        learningTask.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation($"Learning task with id {id} updated");

        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var learningTask = await _context.LearningTasks.FindAsync(id);

        if (learningTask == null) return false;

        _context.LearningTasks.Remove(learningTask);

        await _context.SaveChangesAsync();
        _logger.LogInformation($"Learning task with id {id} deleted");

        return true;
    }



    private LearningTaskResponse MaptoResponse(LearningTask learningTask)
    {
        return new LearningTaskResponse()
        {
            Id = learningTask.Id,
            Title = learningTask.Title,
            Description = learningTask.Description,
            ExpectedTechStack = learningTask.ExpectedTechStack,
            DueDate = learningTask.DueDate,
            Status = learningTask.Status,
            CreatedDate = learningTask.CreatedDate,
            UpdatedDate = learningTask.UpdatedDate
        };
    }

}