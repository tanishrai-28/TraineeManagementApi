using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.TaskAssignmentDTO;

public class CreateTaskAssignmentRequest
{
    [Required]
    public required long TraineeId { get; set; }

    [Required]
    public required long MentorId { get; set; }

    [Required]
    public required long LearningTaskId { get; set; }

    [Required]
    public required DateTime AssignedDate { get; set; }

    [Required]
    public required DateTime DueDate { get; set; }

    public string Remarks { get; set; } = string.Empty;
}