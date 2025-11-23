namespace HabitTracker.Entity;

public class HabitEntry
{
    public int  Id { get; set; }
    
    public DateTime Date { get;  set; }

    public bool IsCompleted { get; set; }

    public int HabitId { get; set; }
    public virtual Habit Habit { get; set; }
}