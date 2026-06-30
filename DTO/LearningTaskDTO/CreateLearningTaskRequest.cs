using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.LearningTaskDTO;

public enum Status
{
    Draft,
    Published,
    Closed
}

public class CreateLearningTaskRequest{
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(50, ErrorMessage = "Title length can't be more than 50.")]
    [MinLength(2, ErrorMessage = "Title length should be min 2.")]
    public required string Title {get; set;}

    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(100, ErrorMessage = "Description length can't be more than 100.")]
    [MinLength(2, ErrorMessage = "Description length should be min 2.")]
    public required string Description {get; set;}

    [Required(ErrorMessage = "ExpectedTechStack is required.")]
    [MaxLength(20, ErrorMessage = "Techstack length can't be more than 20.")]
    [MinLength(2, ErrorMessage = "Techstack length should be min 2.")]
    public required string ExpectedTechStack {get; set;}

    public required DateTime DueDate {get; set;}

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(Status), ErrorMessage = "Status is not valid")]
    public required string Status {get; set;}
}