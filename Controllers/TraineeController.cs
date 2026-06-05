using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Models;

namespace TranineeManagementApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TraineesController: ControllerBase {
        private static List<Trainee> trainees = new List<Trainee>();
        private static int nextId = 1;

        [HttpGet]
        public List<Trainee> GetAll() {
            return trainees;
        }

        [HttpGet("{id}")] 
        public Trainee GetTrainee(int id) {
            return trainees.FirstOrDefault(trainee => trainee.Id == id);
        }

        [HttpPost]
        public Trainee Create(Trainee trainee) {
            trainee.Id = nextId++;

            trainee.CreatedDate = DateTime.UtcNow;
            trainee.UpdatedDate = DateTime.UtcNow;

            trainees.Add(trainee);
            return trainee;
        }

    }
}