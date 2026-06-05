using TraineeManagementApi.DTO;

namespace TraineeManagementApi.Services {

    public interface ITraineeService{
        List<TraineeResponse> GetAll();

        TraineeResponse GetById(long id);

        TraineeResponse Create(CreateTraineeRequest request);

        bool Update(long id, UpdateTraineeRequest request);

        bool Delete(long id);
    }

}