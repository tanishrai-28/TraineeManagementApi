using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Context;
using TraineeManagementApi.DTO.MentorDTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services.Interface;
using TraineeManagementApi.Services.Redis;

namespace TraineeManagementApi.Services;

public class MentorService : IMentorService
{
    private readonly ILogger<MentorService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cache;

    public MentorService(ApplicationDbContext context, ICacheService cache, ILogger<MentorService> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<MentorResponse>> GetAllAsync()
    {
        var mentors = await _context.Mentors.ToListAsync();

        return [.. mentors.Select(MaptoResponse)];
    }

    public async Task<MentorResponse?> GetByIdAsync(long id)
    {
        var cachedKey = $"mentor:{id}";
        MentorResponse? cachedData = await _cache.GetAsync<MentorResponse>(cachedKey);

        if (cachedData != null)
        {
            _logger.LogInformation("Hit for mentor");
            return cachedData;
        }

        _logger.LogInformation("Miss for mentor");

        var mentor = await _context.Mentors.FindAsync(id);

        if (mentor == null)
        {
            _logger.LogInformation($"Mentor with {id} not found");
        }

        var response = MaptoResponse(mentor!);

        await _cache.SetAsync(cachedKey, response);

        return response;
    }

    public async Task<MentorResponse> CreateAsync(CreateMentorRequest request)
    {
        var mentor = new Mentor()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Expertise = request.Expertise,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _context.Mentors.AddAsync(mentor);

        await _context.SaveChangesAsync();

        _logger.LogInformation($"Mentor with email {request.Email} created.");

        return MaptoResponse(mentor);
    }

    public async Task<bool> UpdateAsync(long id, UpdateMentorRequest request)
    {
        var cachedKey = $"mentor:{id}";
        var mentor = await _context.Mentors.FindAsync(id);

        if (mentor == null) return false;

        mentor.FirstName = request.FirstName;
        mentor.LastName = request.LastName;
        mentor.Email = request.Email;
        mentor.Expertise = request.Expertise;
        mentor.Status = request.Status;
        mentor.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation($"Mentor id {id} updated");

        await _cache.RemoveAsync(cachedKey);

        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var cachedKey = $"mentor:{id}";
        var mentor = await _context.Mentors.FindAsync(id);

        if (mentor == null) return false;

        _context.Mentors.Remove(mentor);

        await _context.SaveChangesAsync();
        _logger.LogInformation($"Mentor with id {id} deleted");

        await _cache.RemoveAsync(cachedKey);

        return true;
    }

    private MentorResponse MaptoResponse(Mentor mentor)
    {
        return new MentorResponse()
        {
            Id = mentor.Id,
            FirstName = mentor.FirstName,
            LastName = mentor.LastName,
            Email = mentor.Email,
            Expertise = mentor.Expertise,
            Status = mentor.Status,
            CreatedDate = mentor.CreatedDate,
            UpdatedDate = mentor.UpdatedDate
        };
    }
}