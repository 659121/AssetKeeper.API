using CoreLogic;
using CoreLogic.Models.DTO.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegistrationAsync([FromBody] RegisterDto registerDto)
    {
        // TODO добавить проверку на уникальность имени
        await authService.RegisterAsync(registerDto);
        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        var loginResult = await authService.LoginAsync(loginDto);

        return 
            loginResult.Success
            ? Ok(new { token = loginResult.Token })
            : BadRequest(loginResult.Error);
    }
}
