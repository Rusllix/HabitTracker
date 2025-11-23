using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HabitTracker;
using HabitTracker.Entity;
using HabitTracker.Exceptions;
using HabitTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
    string GenerateJwt(LoginDto dto);
}
public class AccountService : IAccountService
{
    private readonly TrackerDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public AccountService(TrackerDbContext dbContext,
                          IPasswordHasher<User> passwordHasher,
                          AuthenticationSettings authenticationSettings)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }

    public void RegisterUser(RegisterUserDto dto)
    {
        // Sprawdzenie czy email już istnieje
        if (_dbContext.Users.Any(u => u.Email == dto.Email))
            throw new BadRequestException("Email is already in use");

        var newUser = new User
        {
            Email = dto.Email,
            UserName = dto.UserName,
            Nationality = dto.Nationality,
            RoleId = dto.RoleId,
            CreatedAt = DateTime.UtcNow
        };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
    }

    public string GenerateJwt(LoginDto dto)
    {
        var user = _dbContext.Users.Include(u => u.Role)
                    .FirstOrDefault(u => u.Email == dto.Email);

        if (user == null)
            throw new BadRequestException("Invalid email or password");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid email or password");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        if (!string.IsNullOrEmpty(user.Nationality))
            claims.Add(new Claim("Nationality", user.Nationality));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(
            _authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
