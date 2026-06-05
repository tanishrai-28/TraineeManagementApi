using TraineeManagementApi.DTO;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services;

namespace TraineeManagementApi.Services {
    public class TraineeService: ITraineeService {
        private static List<Trainee> trainees = new List<Trainee>();
        private static int nextId = 1;

        public List<TraineeResponse> GetAll() {
            return trainees.Select(MaptoResponse).ToList();
        }

        public TraineeResponse GetById (long id) {
            var trainee = trainees.FirstOrDefault(trainee => trainee.Id == id);

            return trainee == null ? null : MaptoResponse(trainee);
        }

        public TraineeResponse Create(CreateTraineeRequest request) {
            var trainee = new Trainee(){
                Id= nextId++,
                FirstName= request.FirstName,
                LastName= request.LastName,
                Email= request.Email,
                TechStack= request.TechStack,
                Status= request.Status,
                CreatedDate= DateTime.UtcNow,
                UpdatedDate= DateTime.UtcNow
            };

            trainees.Add(trainee);
            return MaptoResponse(trainee);
        }

        public bool Update(long id, UpdateTraineeRequest request) {
            var trainee = trainees.FirstOrDefault(trainee => trainee.Id == id);

            if(trainee == null) return false;

            trainee.FirstName = request.FirstName;
            trainee.LastName = request.LastName;
            trainee.Email = request.Email;
            trainee.TechStack = request.TechStack;
            trainee.Status = request.Status;

            return true;
        }

        public bool Delete(long id) {
            var trainee = trainees.FirstOrDefault(trainee => trainee.Id == id);

            if(trainee == null) return false;

            trainees.Remove(trainee);

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