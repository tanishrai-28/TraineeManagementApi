using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace TraineeManagementApi.DTO;

public class UpdateTraineeRequest{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "First name length can't be more than 50.")]
    public string FirstName {get; set;}

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Last name length can't be more than 50.")]
    public string LastName {get; set;}

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Valid email is required")]
    public string Email {get; set;}

    [Required(ErrorMessage = "Tech stack is required.")]
    public string TechStack {get; set;}

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(TraineeStatus), ErrorMessage = "Invalid status")]
    public string Status {get; set;}
}