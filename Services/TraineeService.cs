using TraineeManagementApi.DTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Context;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagementApi.Services
{
    public class TraineeService : ITraineeService
    {
        private readonly ApplicationDbContext _context;

        public TraineeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TraineeResponse>> GetAllAsync(string search)
        {
            try
            {
                var trainees = await _context.Trainees.ToListAsync();

                if (search != "")
                {
                    var results = trainees.Where(t => t.FirstName.Contains(search) || t.LastName.Contains(search) || t.Email.Contains(search) || t.TechStack.Contains(search)).ToList();
                    return results.Select(MaptoResponse).ToList();
                }

                return trainees.Select(MaptoResponse).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error while getting all trainees ", e);
            }

        }

        public async Task<TraineeResponse> GetByIdAsync(long id)
        {
            try
            {
                var trainee = await _context.Trainees.FindAsync(id);

                return trainee == null ? null : MaptoResponse(trainee);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to retrieve trianee with id {id} ", e);
            }

        }

        public async Task<TraineeResponse> CreateAsync(CreateTraineeRequest request)
        {
            try
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

                return MaptoResponse(trainee);
            }
            catch (Exception e)
            {

                throw new Exception("Failed to create trainee -> ", e);
            }
        }

        public async Task<bool> UpdateAsync(long id, UpdateTraineeRequest request)
        {
            try
            {
                var trainee = await _context.Trainees.FindAsync(id);

                if (trainee == null) return false;

                trainee.FirstName = request.FirstName;
                trainee.LastName = request.LastName;
                trainee.Email = request.Email;
                trainee.TechStack = request.TechStack;
                trainee.Status = request.Status;
                trainee.UpdatedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {

                throw new Exception($"Error while updating trainee with id {id} -> ", e);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var trainee = await _context.Trainees.FindAsync(id);

                if (trainee == null) return false;

                _context.Trainees.Remove(trainee);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {

                throw new Exception($"Failed to delete trainee with id {id} -> ", e);
            }
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