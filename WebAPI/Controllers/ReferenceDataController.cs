using CoreLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/reference")]
public class ReferenceDataController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public ReferenceDataController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("departments")]
    public async Task<ActionResult<List<CoreLogic.Domain.Department>>> GetDepartments(CancellationToken ct = default)
    {
        var departments = await _inventoryService.GetDepartmentsAsync(ct);
        return Ok(departments);
    }

    [HttpGet("statuses")]
    public async Task<ActionResult<List<CoreLogic.Domain.DeviceStatus>>> GetStatuses(CancellationToken ct = default)
    {
        var statuses = await _inventoryService.GetDeviceStatusesAsync(ct);
        return Ok(statuses);
    }

    [HttpGet("reasons")]
    public async Task<ActionResult<List<CoreLogic.Domain.MovementReason>>> GetReasons(CancellationToken ct = default)
    {
        var reasons = await _inventoryService.GetMovementReasonsAsync(ct);
        return Ok(reasons);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<List<CoreLogic.Models.DepartmentStatsDto>>> GetStatistics(CancellationToken ct = default)
    {
        var stats = await _inventoryService.GetDepartmentStatisticsAsync(ct);
        return Ok(stats);
    }

    [HttpPost("departments")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateDepartment([FromBody] CreateDepartmentRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Department name is required");

        var department = new CoreLogic.Domain.Department
        {
            Code = request.Code,
            Name = request.Name.Trim(),
            IsActive = true
        };

        await _inventoryService.CreateDepartmentAsync(department, ct);

        return CreatedAtAction(nameof(GetDepartments), new { id = department.Id }, department.Id);
    }

    [HttpPost("reasons")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Guid>> CreateReason([FromBody] CreateReasonRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
            return BadRequest("Reason code is required");
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Reason name is required");
        var result = await _inventoryService.CreateReasonAsync(
            new CoreLogic.Domain.MovementReason
            {
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                Description = request.Description?.Trim() ?? string.Empty,
                SortOrder = request.SortOrder,
                IsActive = true
            },
            ct);
        
        return result ? Ok() : BadRequest();
    }
}