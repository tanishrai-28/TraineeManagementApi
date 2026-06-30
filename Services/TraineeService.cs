using TraineeManagementApi.DTO.TraineeDTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Context;
using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.DTO.Pagination;
using TraineeManagementApi.Helpers;
using TraineeManagementApi.Services.Interface;
using TraineeManagementApi.Services.Redis;

namespace TraineeManagementApi.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly ILogger<TraineeService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _cache;

        public TraineeService(ApplicationDbContext context, ICacheService cache, ILogger<TraineeService> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<TraineeResponse>> GetAllAsync(TraineeQueryFilter filter, CancellationToken cancellationToken = default)
        {
            var pageNumber = Math.Max(1, filter.PageNumber);
            var pageSize = Math.Clamp(filter.PageSize, 1, 50);

            var query = _context.Trainees.AsNoTracking().AsQueryable();

            // search
            query = query.ApplySearch(filter.Search);
            query = query.ApplyFilter(filter.Status);

            var totalRecords = await query.CountAsync(cancellationToken);

            var trainees = await query
                .ApplyPagination(pageNumber, pageSize)
                .Select(t => new TraineeResponse
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email,
                    TechStack = t.TechStack,
                    Status = t.Status
                })
                .ToListAsync();

            _logger.LogInformation("Fetched all trainees");

            return trainees;
        }

        public async Task<TraineeResponse?> GetByIdAsync(long id)
        {
            var cachedKey = $"trainee:{id}";

            TraineeResponse? cachedData = await _cache.GetAsync<TraineeResponse>(cachedKey);

            if (cachedData != null)
            {
                _logger.LogInformation("Hit for trainee");
                return cachedData;
            }

            _logger.LogInformation("Miss for trainee");


            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null)
            {
                _logger.LogWarning($"Trainee with {id} not found");
                return null;
            }

            var response = MaptoResponse(trainee);

            await _cache.SetAsync(cachedKey, response, 2);
            _logger.LogInformation($"Fetched trainee with id: {id} and saved to cache");

            return response;
        }

        public async Task<TraineeResponse> CreateAsync(CreateTraineeRequest request)
        {
            var trainee = new Trainee()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                TechStack = request.TechStack,
                Status = request.Status,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _context.Trainees.AddAsync(trainee);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"User with email {request.Email} created.");

            return MaptoResponse(trainee);
        }

        public async Task<bool> UpdateAsync(long id, UpdateTraineeRequest request)
        {
            var cachedKey = $"trainee:{id}";
            var trainee = await _context.Trainees.FindAsync(id);

            if (trainee == null)
            {
                _logger.LogWarning("Trainee record not found");
                return false;
            }

            trainee.FirstName = request.FirstName;
            trainee.LastName = request.LastName;
            trainee.Email = request.Email;
            trainee.TechStack = request.TechStack;
            trainee.Status = request.Status;
            trainee.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _cache.RemoveAsync(cachedKey);
            _logger.LogInformation($"Trainee with id: {id} updated and removed from cache");

            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var cachedKey = $"trainee:{id}";
            var trainee = await _context.Trainees.FindAsync(id);

            if (trainee == null) {
                _logger.LogWarning($"Trainee to be deleted not found");
                return false;
            }

            _context.Trainees.Remove(trainee);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync(cachedKey);
            _logger.LogInformation($"Trainee with id {id} deleted and removed from cache");

            return true;
        }

        private TraineeResponse MaptoResponse(Trainee trainee)
        {
            return new TraineeResponse()
            {
                Id = trainee.Id,
                FirstName = trainee.FirstName,
                LastName = trainee.LastName,
                Email = trainee.Email,
                TechStack = trainee.TechStack,
                Status = trainee.Status,
                CreatedDate = trainee.CreatedDate,
                UpdatedDate = trainee.UpdatedDate
            };
        }
    }
}