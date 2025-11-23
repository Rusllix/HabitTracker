using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Models;

public class UpdateHabitDto
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Frequency { get; set; }
}