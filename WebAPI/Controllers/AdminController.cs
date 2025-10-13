using CoreLogic;
using CoreLogic.Models.DTO.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await adminService.GetUsersAsync();
        return Ok(users);
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUserDetails(int id)
    {
        var user = await adminService.GetUserDetailsAsync(id);
        return user != null 
            ? Ok(user) 
            : NotFound();
    }

    [HttpPatch("users/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var result = await adminService.UpdateUserAsync(id, request);
        return result 
            ? Ok() 
            : NotFound();
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await adminService.DeleteUserAsync(id);
        return result 
            ? NoContent() 
            : NotFound();
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await adminService.GetRolesAsync();
        return Ok(roles);
    }
}
