using HabitTracker.Exceptions;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            _accountService.RegisterUser(dto);
            return Ok(new { Message = "User registered successfully" });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = _accountService.GenerateJwt(dto);
            return Ok(new { Token = token });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}