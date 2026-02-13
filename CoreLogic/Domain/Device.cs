namespace CoreLogic.Domain;
public class Device
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? InventoryNumber { get; set; }
    public string? Description { get; set; }
    public string? Sticker { get; set; }
    
    // Текущее местоположение и статус (кэш)
    public Guid? CurrentDepartmentId { get; set; }
    public Department? CurrentDepartment { get; set; }
    
    public int CurrentStatusId { get; set; }
    public DeviceStatus? CurrentStatus { get; set; }
    
    // Навигационные свойства
    public ICollection<DeviceMovement> Movements { get; set; } = new List<DeviceMovement>();
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}