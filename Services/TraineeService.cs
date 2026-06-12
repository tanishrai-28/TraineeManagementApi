using TraineeManagementApi.DTO.TraineeDTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Context;
using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.DTO.Pagination;
using TraineeManagementApi.Helpers;
using TraineeManagementApi.Services.Interface;

namespace TraineeManagementApi.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly ILogger<TraineeService> _logger;
        private readonly ApplicationDbContext _context;

        public TraineeService(ApplicationDbContext context, ILogger<TraineeService> logger)
        {
            _context = context;
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

            return trainees;
        }

        public async Task<TraineeResponse?> GetByIdAsync(long id)
        {
            // try
            // {
            var trainee = await _context.Trainees.FindAsync(id);

            if (trainee == null)
            {
                _logger.LogInformation($"Trainee with {id} not found");
            }

            return trainee == null ? null : MaptoResponse(trainee);
            // }
            // catch (Exception e)
            // {
            //     throw new Exception($"Failed to retrieve trianee with id {id} ", e);
            // }

        }

        public async Task<TraineeResponse> CreateAsync(CreateTraineeRequest request)
        {
            // try
            // {
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
            // }
            // catch (Exception e)
            // {

            //     throw new Exception("Failed to create trainee -> ", e);
            // }
        }

        public async Task<bool> UpdateAsync(long id, UpdateTraineeRequest request)
        {
            // try
            // {
            var trainee = await _context.Trainees.FindAsync(id);

            if (trainee == null) return false;

            trainee.FirstName = request.FirstName;
            trainee.LastName = request.LastName;
            trainee.Email = request.Email;
            trainee.TechStack = request.TechStack;
            trainee.Status = request.Status;
            trainee.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            _logger.LogInformation($"User id {id} updated");

            return true;
            // }
            // catch (Exception e)
            // {

            //     throw new Exception($"Error while updating trainee with id {id} -> ", e);
            // }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            // try
            // {
            var trainee = await _context.Trainees.FindAsync(id);

            if (trainee == null) return false;

            _context.Trainees.Remove(trainee);

            await _context.SaveChangesAsync();
            _logger.LogInformation($"User with id {id} deleted");

            return true;
            // }
            // catch (Exception e)
            // {

            //     throw new Exception($"Failed to delete trainee with id {id} -> ", e);
            // }
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