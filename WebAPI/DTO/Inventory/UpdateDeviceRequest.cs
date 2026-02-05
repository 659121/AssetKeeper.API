namespace WebAPI.DTO.Inventory;

public class UpdateDeviceRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? InventoryNumber { get; set; }
    public string? Description { get; set; }
}