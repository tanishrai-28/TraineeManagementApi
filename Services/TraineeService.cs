using TraineeManagementApi.DTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Context;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagementApi.Services {
    public class TraineeService: ITraineeService {
        private readonly ApplicationDbContext _context;

        public TraineeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TraineeResponse>> GetAllAsync(string search) {
            var trainees = await _context.Trainees.ToListAsync();

            if(search != "")
            {
                var results = trainees.Where(t => t.FirstName.Contains(search) || t.LastName.Contains(search) || t.Email.Contains(search) || t.TechStack.Contains(search)).ToList();
                return results.Select(MaptoResponse).ToList();
            }

            return trainees.Select(MaptoResponse).ToList();
        }

        public async Task<TraineeResponse> GetByIdAsync (long id) {
            var trainee = await _context.Trainees.FindAsync(id);

            return trainee == null ? null : MaptoResponse(trainee);
        }

        public async Task<TraineeResponse> CreateAsync(CreateTraineeRequest request) {
            var trainee = new Trainee(){
                FirstName= request.FirstName,
                LastName= request.LastName,
                Email= request.Email,
                TechStack= request.TechStack,
                Status= request.Status,
                CreatedDate= DateTime.UtcNow,
                UpdatedDate= DateTime.UtcNow
            };

            await _context.Trainees.AddAsync(trainee);

            await _context.SaveChangesAsync();

            return MaptoResponse(trainee);
        }

        public async Task<bool> UpdateAsync(long id, UpdateTraineeRequest request) {
            var trainee = await _context.Trainees.FindAsync(id);

            if(trainee == null) return false;

            trainee.FirstName = request.FirstName;
            trainee.LastName = request.LastName;
            trainee.Email = request.Email;
            trainee.TechStack = request.TechStack;
            trainee.Status = request.Status;
            trainee.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(long id) {
            var trainee = await _context.Trainees.FindAsync(id);

            if(trainee == null) return false;

            _context.Trainees.Remove(trainee);

            await _context.SaveChangesAsync();

            return true;
        }

        private TraineeResponse MaptoResponse(Trainee trainee) {
            return new TraineeResponse (){
                Id= trainee.Id,
                FirstName= trainee.FirstName,
                LastName= trainee.LastName,
                Email= trainee.Email,
                TechStack= trainee.TechStack,
                Status= trainee.Status,
                CreatedDate= trainee.CreatedDate,
                UpdatedDate= trainee.UpdatedDate
            };
        }
    }
}