using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.SubmissionDTO;

public class CreateSubmissionRequest
{
    [Required]
    public required long TaskAssignmentId { get; set; }

    [Required]
    public required string SubmissionUrl { get; set; }

    public string Notes { get; set; } = string.Empty;
}