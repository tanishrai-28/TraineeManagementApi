using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.LearningTaskDTO;

public class UpdateLearningTaskRequest{
    [Required(ErrorMessage = "Title is required.")]
    // [MaxLength(50, ErrorMessage = "First name length can't be more than 50.")]
    // [MinLength(2, ErrorMessage = "First name length should be min 2.")]
    public required string Title {get; set;}

    [Required(ErrorMessage = "Description is required.")]
    // [MaxLength(50, ErrorMessage = "Last name length can't be more than 50.")]
    // [MinLength(2, ErrorMessage = "Last name length should be min 2.")]
    public required string Description {get; set;}

    [Required(ErrorMessage = "ExpectedTechStack is required.")]
    public required string ExpectedTechStack {get; set;}

    public required DateTime DueDate {get; set;}

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(Status), ErrorMessage = "Status is not valid")]
    public required string Status {get; set;}
}