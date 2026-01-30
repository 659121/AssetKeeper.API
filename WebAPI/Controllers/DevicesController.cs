using CoreLogic.Models;
using CoreLogic.Services;
using WebAPI.DTO.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/devices")]
public class DevicesController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DevicesController(IInventoryService inventoryService, IHttpContextAccessor httpContextAccessor)
    {
        _inventoryService = inventoryService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<DeviceDto>>> GetDevices([FromQuery] DeviceFilter filter, CancellationToken ct = default)
    {
        filter.Validate();
        var result = await _inventoryService.GetDevicesAsync(filter, ct);
        
        var dtos = result.Items.Select(MapToDeviceDto).ToList();
        return Ok(new PagedResult<DeviceDto>
        {
            Items = dtos,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDto>> GetDevice(Guid id, CancellationToken ct = default)
    {
        var device = await _inventoryService.GetDeviceByIdAsync(id, ct);
        if (device == null) return NotFound();
        
        return Ok(MapToDeviceDto(device));
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<List<DeviceMovementDto>>> GetDeviceHistory(Guid id, CancellationToken ct = default)
    {
        var history = await _inventoryService.GetDeviceHistoryAsync(id, ct);
        return Ok(history);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateDevice([FromBody] CreateDeviceRequest request, CancellationToken ct = default)
    {
        var device = new CoreLogic.Domain.Device
        {
            Name = request.Name,
            InventoryNumber = request.InventoryNumber,
            Description = request.Description,
            CurrentDepartmentId = request.CurrentDepartmentId,
            CurrentStatusId = request.CurrentStatusId
        };

        var id = await _inventoryService.CreateDeviceAsync(device, ct);
        return CreatedAtAction(nameof(GetDevice), new { id }, id);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDevice([FromBody] UpdateDeviceRequest request, CancellationToken ct = default)
    {
        var device = new CoreLogic.Domain.Device
        {
            Id = request.Id,
            Name = request.Name,
            InventoryNumber = request.InventoryNumber,
            Description = request.Description,
            CurrentDepartmentId = request.CurrentDepartmentId,
            CurrentStatusId = request.CurrentStatusId
        };

        var result = await _inventoryService.UpdateDeviceAsync(device, ct);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(Guid id, CancellationToken ct = default)
    {
        var result = await _inventoryService.DeleteDeviceAsync(id, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("move")]
    public async Task<IActionResult> MoveDevice([FromBody] MoveDeviceRequest request, CancellationToken ct = default)
    {
        var username = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "system";
        var result = await _inventoryService.MoveDeviceAsync(
            request.DeviceId, 
            request.ToDepartmentId, 
            request.ReasonId, 
            username, 
            request.Note, 
            ct);
        
        return result ? Ok() : BadRequest();
    }

    private DeviceDto MapToDeviceDto(CoreLogic.Domain.Device device)
    {
        return new DeviceDto
        {
            Id = device.Id,
            Name = device.Name,
            InventoryNumber = device.InventoryNumber,
            Description = device.Description,
            CurrentDepartmentId = device.CurrentDepartmentId,
            CurrentDepartmentName = device.Movements.LastOrDefault()?.ToDepartment?.Name,
            CurrentStatusId = device.CurrentStatusId,
            CurrentStatusName = "Unknown", // В реальном проекте нужно получить из справочника
            CreatedAt = device.CreatedAt,
            UpdatedAt = device.UpdatedAt,
            IsActive = device.IsActive
        };
    }
}