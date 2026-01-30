namespace WebAPI.DTO.Inventory;

public class CreateDeviceRequest
{
    public string Name { get; set; } = null!;
    public string? InventoryNumber { get; set; }
    public string? Description { get; set; }
    public Guid? CurrentDepartmentId { get; set; }
    public int CurrentStatusId { get; set; }
}