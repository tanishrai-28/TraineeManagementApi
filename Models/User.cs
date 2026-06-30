using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagementApi.Models;

public enum RoleType
{
    Admin, 
    Mentor, 
    Trainee
}

[Index(nameof(Username), IsUnique = true)]
public class User{
    [Key]
    public long Id {get; set;}

    [Required(ErrorMessage = "Username is required.")]
    [MaxLength(50, ErrorMessage = "Username length can't be more than 50.")]
    [MinLength(5, ErrorMessage = "Username length should be min 5.")]
    public required string Username {get; set;}

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Valid email is required")]
    public required string Email {get; set;}

    [Required(ErrorMessage = "Password is required.")]
    public required string PasswordHash {get; set;}

    [Required(ErrorMessage = "Role is required.")]
    [EnumDataType(typeof(RoleType), ErrorMessage = "Role is not valid")]
    public required RoleType Role {get; set;}
    
    public DateTime CreatedDate {get; set;}

    public DateTime UpdatedDate {get; set;}
}