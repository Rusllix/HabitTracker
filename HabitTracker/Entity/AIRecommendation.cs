namespace HabitTracker.Entity;

public class AIRecommendation
{
    public int Id { get; set; }
    
    public string Type { get; set; } 
    
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public int UserId { get; set; }
    public virtual User User { get; set; }
    
}