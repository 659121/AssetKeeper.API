using CoreLogic.Models;
using CoreLogic.Services;
using WebAPI.DTO.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [Authorize(Roles = "User")]
    public async Task<ActionResult<Guid>> CreateDevice([FromBody] CreateDeviceRequest request, CancellationToken ct = default)
    {
        var username = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "system";
        var device = new CoreLogic.Domain.Device
        {
            Name = request.Name,
            InventoryNumber = request.InventoryNumber,
            Description = request.Description,
            Sticker = request.Sticker,
            CurrentDepartmentId = request.CurrentDepartmentId,
            CurrentStatusId = request.CurrentStatusId
        };

        var id = await _inventoryService.CreateDeviceAsync(device, username, ct);
        return CreatedAtAction(nameof(GetDevice), new { id }, id);
    }

    [HttpPut]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateDevice([FromBody] UpdateDeviceRequest request, CancellationToken ct = default)
    {
        var device = new CoreLogic.Domain.Device
        {
            Id = request.Id,
            Name = request.Name,
            InventoryNumber = request.InventoryNumber,
            Description = request.Description
        };

        var result = await _inventoryService.UpdateDeviceAsync(device, ct);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> DeleteDevice(Guid id, CancellationToken ct = default)
    {
        var result = await _inventoryService.DeleteDeviceAsync(id, ct);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("move")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MoveDevice([FromBody] MoveDeviceRequest request, CancellationToken ct = default)
    {
        var username = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "system";
        var result = await _inventoryService.MoveDeviceAsync(
            request.DeviceId, 
            request.ToDepartmentId, 
            request.ReasonId, 
            username, 
            request.NewSticker,
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
            Sticker = device.Sticker,  // Стикер
            CurrentDepartmentId = device.CurrentDepartmentId,
            CurrentDepartmentName = device.CurrentDepartment?.Name ?? "unknown",
            CurrentStatusId = device.CurrentStatusId,
            CurrentStatusName = device.CurrentStatus?.Name ?? "Unknown",
            CreatedAt = device.CreatedAt,
            UpdatedAt = device.UpdatedAt,
            IsActive = device.IsActive
        };
    }
}