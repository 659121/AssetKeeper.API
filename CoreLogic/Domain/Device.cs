namespace CoreLogic.Domain;

public class Device
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? InventoryNumber { get; set; }
    public string? SerialNumber { get; set; }
    public string? Description { get; set; }
    
    public int? CurrentDepartmentId { get; set; }
    public int CurrentStatusId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}