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
        var registerResult = await authService.RegisterAsync(registerDto);

        return
            registerResult.Success
            ? Created()
            : BadRequest(registerResult.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        var loginResult = await authService.LoginAsync(loginDto);

        return 
            loginResult.Success
            ? Ok(new { token = loginResult.Token })
            : BadRequest(loginResult.Message);
    }
}
