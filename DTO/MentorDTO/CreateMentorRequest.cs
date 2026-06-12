using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.DTO.MentorDTO;

public enum Status
{
    Active,
    Inactive
}

public class CreateMentorRequest{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "First name length can't be more than 50.")]
    [MinLength(2, ErrorMessage = "First name length should be min 2.")]
    public required string FirstName {get; set;}

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Last name length can't be more than 50.")]
    [MinLength(2, ErrorMessage = "Last name length should be min 2.")]
    public required string LastName {get; set;}

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Valid email is required")]
    public required string Email {get; set;}

    [Required(ErrorMessage = "Expertise is required.")]
    public required string Expertise {get; set;}

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(Status), ErrorMessage = "Status is not valid")]
    public required string Status {get; set;}
}