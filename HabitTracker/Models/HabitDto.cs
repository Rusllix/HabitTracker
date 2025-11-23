namespace HabitTracker.Models;

public class HabitDto
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Frequency { get; set; }
}