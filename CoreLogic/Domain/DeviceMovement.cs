namespace CoreLogic.Domain;
public class DeviceMovement
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public Device? Device { get; set; }
    
    public Guid? FromDepartmentId { get; set; }
    public Department? FromDepartment { get; set; }
    
    public Guid ToDepartmentId { get; set; }
    public Department? ToDepartment { get; set; }
    
    public int? OldSticker { get; set; }
    public int? NewSticker { get; set; }
    
    public Guid ReasonId { get; set; }
    public MovementReason? Reason { get; set; }
    
    public DateTime MovedAt { get; set; }
    public required string MovedBy { get; set; }
    public string? Note { get; set; }
}