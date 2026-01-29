using CoreLogic.Models.Admin;
using CoreLogic.Services;
using CoreLogic.Interfaces;
using WebAPI.DTO.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(CancellationToken ct = default)
    {
        var users = await _adminService.GetUsersAsync(ct);
        var dtos = users.Select(u => new UserDetailsDto(
            u.Id,
            u.Username,
            u.RegDate,
            u.LastLogin,
            u.IsActive,
            u.Roles
        )).ToList();
        return Ok(dtos);
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUserDetails(int id, CancellationToken ct = default)
    {
        var user = await _adminService.GetUserDetailsAsync(id, ct);

        if (user == null)
            return NotFound();

        // Маппинг: доменная модель → внешний DTO
        var dto = new UserDetailsDto(
            user.Id,
            user.Username,
            user.RegDate,
            user.LastLogin,
            user.IsActive,
            user.Roles
        );

        return Ok(dto);
    }

    [HttpPatch("users/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request, CancellationToken ct = default)
    {
        // Маппинг: внешний запрос → внутренняя команда
        var command = new UpdateUserCommand
        {
            IsActive = request.IsActive,
            Roles = request.Roles
        };

        bool result = await _adminService.UpdateUserAsync(id, command, ct);
        
        return result ? Ok() : NotFound();
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken ct = default)
    {
        var result = await _adminService.DeleteUserAsync(id, ct);
        return result 
            ? NoContent() 
            : NotFound();
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles(CancellationToken ct = default)
    {
        var roles = await _adminService.GetRolesAsync(ct);
        return Ok(roles);
    }
}