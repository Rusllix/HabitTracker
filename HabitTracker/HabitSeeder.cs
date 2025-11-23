using HabitTracker.Entity;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker
{
    public class HabitSeeder
    {
        private readonly TrackerDbContext _dbContext;

        public HabitSeeder(TrackerDbContext dbContext)
        {
            _dbContext = dbContext; // połączenie do bazy
        }

        public void Seed()
        {
            if (!_dbContext.Database.CanConnect()) return;

            // 1️⃣ Role
            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }

            // 2️⃣ Użytkownicy
            if (!_dbContext.Users.Any())
            {
                var users = GetUsers();
                _dbContext.Users.AddRange(users);
                _dbContext.SaveChanges();
            }

            // 3️⃣ Habits
            if (!_dbContext.Habits.Any())
            {
                var habits = GetHabits();
                _dbContext.Habits.AddRange(habits);
                _dbContext.SaveChanges();
            }

            // 4️⃣ HabitEntries
            if (!_dbContext.HabitEntries.Any())
            {
                var entries = GetHabitEntries();
                _dbContext.HabitEntries.AddRange(entries);
                _dbContext.SaveChanges();
            }

            // 5️⃣ AIRecommendations
            if (!_dbContext.AIRecommendations.Any())
            {
                var recommendations = GetAIRecommendations();
                _dbContext.AIRecommendations.AddRange(recommendations);
                _dbContext.SaveChanges();
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Name = "User" },
                new Role { Name = "Admin" }
            };
        }

        private IEnumerable<User> GetUsers()
        {
            var adminRoleId = _dbContext.Roles.First(r => r.Name == "Admin").Id;
            var userRoleId = _dbContext.Roles.First(r => r.Name == "User").Id;

            return new List<User>
            {
                new User
                {
                    Email = "admin@example.com",
                    UserName = "Admin",
                    PasswordHash = "hashedPassword",
                    CreatedAt = DateTime.UtcNow,
                    RoleId = adminRoleId
                },
                new User
                {
                    Email = "user@example.com",
                    UserName = "User",
                    PasswordHash = "hashedPassword",
                    CreatedAt = DateTime.UtcNow,
                    RoleId = userRoleId
                }
            };
        }

        private IEnumerable<Habit> GetHabits()
        {
            var userId = _dbContext.Users.First(u => u.UserName == "User").Id;
            return new List<Habit>
            {
                new Habit
                {
                    Title = "Codzienne ćwiczenia",
                    Description = "15 minut treningu",
                    Frequency = 1,
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId
                },
                new Habit
                {
                    Title = "Picie wody",
                    Description = "2 litry dziennie",
                    Frequency = 1,
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId
                }
            };
        }

        private IEnumerable<HabitEntry> GetHabitEntries()
        {
            var habit = _dbContext.Habits.First(h => h.Title == "Codzienne ćwiczenia");
            return new List<HabitEntry>
            {
                new HabitEntry { HabitId = habit.Id, Date = DateTime.UtcNow.AddDays(-1), IsCompleted = true },
                new HabitEntry { HabitId = habit.Id, Date = DateTime.UtcNow, IsCompleted = false }
            };
        }

        private IEnumerable<AIRecommendation> GetAIRecommendations()
        {
            var user = _dbContext.Users.First(u => u.UserName == "User");
            return new List<AIRecommendation>
            {
                new AIRecommendation
                {
                    Type = "DailyPlan",
                    Content = "Spróbuj wykonać dodatkowe 10 minut ćwiczeń po pracy.",
                    CreatedAt = DateTime.UtcNow,
                    UserId = user.Id
                }
            };
        }
    }
}
