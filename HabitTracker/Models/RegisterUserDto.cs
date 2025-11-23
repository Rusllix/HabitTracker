namespace HabitTracker.Models;

public class RegisterUserDto
{
    public string Email { get; set; }
    public string UserName { get; set; }
    
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Nationality { get; set; }
    public int RoleId { get; set; } = 1;
}