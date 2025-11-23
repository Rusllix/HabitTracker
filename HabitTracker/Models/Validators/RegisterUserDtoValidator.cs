using FluentValidation;
using HabitTracker.Entity;

namespace HabitTracker.Models.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    private readonly TrackerDbContext _dbContext;

    public RegisterUserDtoValidator(TrackerDbContext dbContext)
    {
        _dbContext = dbContext;
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .MinimumLength(6);

        RuleFor(x => x.ConfirmPassword)
            .Equal(e => e.Password);

        RuleFor(e => e.Email)
            .Custom((value, context) =>
            {
                var emailInUse = _dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "Email is already in use");
                }
            });
    }
}