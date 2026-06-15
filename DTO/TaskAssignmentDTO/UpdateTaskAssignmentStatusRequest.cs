using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.TaskAssignmentDTO;

public class UpdateTaskAssignmentStatusRequest
{
    [EnumDataType(typeof(Status), ErrorMessage = "Invalid status")]
    public string? Status { get; set; }
}