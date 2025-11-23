using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using HabitTracker;
using HabitTracker.Entity;
using HabitTracker.MiddleWare;
using HabitTracker.Models;
using HabitTracker.Models.Validators;
using HabitTracker.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var  authenticationSettings = new AuthenticationSettings();
builder.Configuration
    .GetSection("Authentication")
    .Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);


builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

//Services
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddDbContext<TrackerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlserver")));
builder.Services.AddScoped<HabitSeeder>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>(); 



builder.Services.AddScoped<ErrorHandlingMiddleWare>();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RequestTimeMiddleWare>();
var app = builder.Build();

//seeder
using (var serviceScope = app.Services.CreateScope())
{
    var seeder = serviceScope.ServiceProvider.GetRequiredService<HabitSeeder>();
    seeder.Seed();
}

//app methods
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleWare>();
app.UseMiddleware<RequestTimeMiddleWare>();

app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HabitTracker");
});
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();



app.Run();

