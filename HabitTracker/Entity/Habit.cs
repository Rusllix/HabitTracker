namespace HabitTracker.Entity;

public class Habit
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public string? Description { get; set; }
    public int Frequency { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int UserId { get; set; }
    
    public virtual User User { get; set; }
    
    public virtual List<HabitEntry> Entries {get; set;}
}