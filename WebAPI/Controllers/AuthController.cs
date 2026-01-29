using CoreLogic.Interfaces;
using CoreLogic.Models.Auth;
using WebAPI.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegistrationAsync([FromBody] RegisterDto registerDto)
    {
        var command = new RegisterCommand
        {
            Username = registerDto.Username,
            Password = registerDto.Password
        };
        
        var registerResult = await _authService.RegisterAsync(command);

        return
            registerResult.Success
            ? Created()
            : BadRequest(registerResult.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        var command = new LoginCommand
        {
            Username = loginDto.Username,
            Password = loginDto.Password
        };

        LoginResult loginResult = await _authService.LoginAsync(command);
        return 
            loginResult.Success
            ? Ok(new { token = loginResult.Token, lastLogin = loginResult.Message })
            : BadRequest(new { loginResult.Message });
    }
}