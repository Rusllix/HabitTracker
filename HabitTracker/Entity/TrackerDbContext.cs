using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Entity;

public class TrackerDbContext :DbContext
{
        private string _connectionString = "Server=localhost,1433;Database=HabitTrackerDb;User Id=sa;Password=sqlServer2025;TrustServerCertificate=True;";
        
    public TrackerDbContext(DbContextOptions<TrackerDbContext> options)
    : base(options)
    {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<HabitEntry> HabitEntries { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<AIRecommendation> AIRecommendations { get; set; }
    public DbSet<Role> Roles { get; set; }   
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //User Requarmance
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.UserName)
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)        // User ma jedną Role
            .WithMany()                 // Role może mieć wielu użytkowników
            .HasForeignKey(u => u.RoleId)  // Klucz obcy w User
            .IsRequired();     
        //habit Requarmance
        modelBuilder.Entity<Habit>()
            .Property(h => h.Title)
            .IsRequired();
        
        //Habit entry Requarmance
        modelBuilder.Entity<HabitEntry>()
            .Property(e => e.Date)
            .IsRequired();
        
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    } 

}