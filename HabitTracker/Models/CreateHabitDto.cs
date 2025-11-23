using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class CreateHabitDto
{
    [Required]
    public int UserId { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    public string? Description { get; set; }
    

    
    
}