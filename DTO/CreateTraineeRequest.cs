using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO;

public enum TraineeStatus {
    Active,
    Inactive
}

public class CreateTraineeRequest{

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "First name length can't be more than 50.")]
    public required string FirstName {get; set;}

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Last name length can't be more than 50.")]
    public required string LastName {get; set;}

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Valid email is required")]
    public required string Email {get; set;}

    [Required(ErrorMessage = "Tech stack is required.")]
    public required string TechStack {get; set;}

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(TraineeStatus), ErrorMessage = "Invalid status")]
    public required string Status {get; set;}
}