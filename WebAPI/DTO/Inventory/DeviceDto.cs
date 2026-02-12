namespace WebAPI.DTO.Inventory;

public class DeviceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? InventoryNumber { get; set; }
    public string? Description { get; set; }
    public int? Sticker { get; set; }
    public Guid? CurrentDepartmentId { get; set; }
    public string? CurrentDepartmentName { get; set; }
    public int CurrentStatusId { get; set; }
    public string CurrentStatusName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}