using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.TaskAssignmentDTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services.Interface;
using TraineeManagementApi.Services.Redis;

namespace TraineeManagementApi.Services;

public class TaskAssignmentService : ITaskAssignmentService
{
    private readonly ILogger<TaskAssignmentService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cache;

    public TaskAssignmentService(ApplicationDbContext context, ICacheService cache, ILogger<TaskAssignmentService> logger)
    {
        _context = context;
        _logger = logger;
        _cache = cache;
    }

    public async Task<TaskAssignmentResponse> CreateAsync(CreateTaskAssignmentRequest request)
    {
        var traineeExists = await _context.Trainees.AnyAsync(x => x.Id == request.TraineeId);
        if (!traineeExists)
        {
            throw new ArgumentException("Trainee does not exists");
        }

        var mentorExists = await _context.Mentors.AnyAsync(x => x.Id == request.MentorId);
        if (!mentorExists)
        {
            throw new ArgumentException("Mentor does not exists");
        }

        var learningTaskExists = await _context.LearningTasks.AnyAsync(x => x.Id == request.LearningTaskId);
        if (!learningTaskExists)
        {
            throw new ArgumentException("Learning Task does not exists");
        }

        if (request.DueDate.Date < request.AssignedDate.Date)
        {
            throw new ArgumentException("Due date cannot be before assigned date");
        }

        var taskAssignment = new TaskAssignment()
        {
            TraineeId = request.TraineeId,
            MentorId = request.MentorId,
            LearningTaskId = request.LearningTaskId,
            AssignedDate = request.AssignedDate,
            DueDate = request.DueDate,
            Status = "Assigned",
            Remarks = request.Remarks,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _context.TaskAssignments.AddAsync(taskAssignment);

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Task Assignment created.");

        return new TaskAssignmentResponse
        {
            Id = taskAssignment.Id,
            TraineeId = taskAssignment.TraineeId,
            MentorId = taskAssignment.MentorId,
            LearningTaskId = taskAssignment.LearningTaskId,
            AssignedDate = taskAssignment.AssignedDate,
            DueDate = taskAssignment.DueDate,
            Status = taskAssignment.Status,
            Remarks = taskAssignment.Remarks
        };
    }

    public async Task<List<TaskAssignmentResponse>> GetAllAsync()
    {
        var taskAssignments = await _context.TaskAssignments.Select(x => new TaskAssignmentResponse
        {
            Id = x.Id,
            TraineeId = x.TraineeId,
            MentorId = x.MentorId,
            LearningTaskId = x.LearningTaskId,
            AssignedDate = x.AssignedDate,
            DueDate = x.DueDate,
            Status = x.Status,
            Remarks = x.Remarks
        }).ToListAsync();

        return taskAssignments;
    }

    public async Task<TaskAssignmentResponse?> GetByIdAsync(long id)
    {
        var cachedKey = $"task-assignment:{id}";
        TaskAssignmentResponse? cachedData = await _cache.GetAsync<TaskAssignmentResponse>(cachedKey);

        if (cachedData != null)
        {
            _logger.LogInformation("Hit for task assignment");
            return cachedData;
        }

        _logger.LogInformation("Miss for task assignment");

        var taskAssignment = await _context.TaskAssignments.FirstOrDefaultAsync(x => x.Id == id);

        if (taskAssignment == null)
        {
            _logger.LogInformation($"Task Assignment with {id} not found");
            return null;
        }

        var response = new TaskAssignmentResponse
        {
            Id = taskAssignment.Id,
            TraineeId = taskAssignment.TraineeId,
            MentorId = taskAssignment.MentorId,
            LearningTaskId = taskAssignment.LearningTaskId,
            AssignedDate = taskAssignment.AssignedDate,
            DueDate = taskAssignment.DueDate,
            Status = taskAssignment.Status,
            Remarks = taskAssignment.Remarks
        };

        await _cache.SetAsync(cachedKey, response);

        return response;
    }

    public async Task<bool> UpdateStatusAsync(long id, UpdateTaskAssignmentStatusRequest request)
    {
        var cachedKey = $"task-assignment:{id}";
        
        var taskAssignment = await _context.TaskAssignments.FirstOrDefaultAsync(x => x.Id == id);

        if (taskAssignment == null) return false;

        taskAssignment.Status = request.Status;
        taskAssignment.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation($"Task Assignment with id {id} updated");

        await _cache.RemoveAsync(cachedKey);

        return true;
    }
}