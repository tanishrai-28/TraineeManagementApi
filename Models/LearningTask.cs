using System.ComponentModel.DataAnnotations;

namespace TraineeManagementApi.Models;

public class LearningTask{
    [Key]
    public long Id {get; set;}

    public required string Title {get; set;}

    public required string Description {get; set;}

    public required string ExpectedTechStack {get; set;}

    public required DateTime DueDate {get; set;}

    public required string Status {get; set;}
    
    public DateTime CreatedDate {get; set;}

    public DateTime UpdatedDate {get; set;}
}