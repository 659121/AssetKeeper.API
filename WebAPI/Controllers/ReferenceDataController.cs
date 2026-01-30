using CoreLogic.Services;
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
}