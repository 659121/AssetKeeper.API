namespace DataAccess.Entities;
public class DeviceMovement
{
    public Guid Id { get; set; }
    public int DeviceId { get; set; }
    public Device? Device { get; set; }
    
    public int? FromDepartmentId { get; set; }
    public Department? FromDepartment { get; set; }
    
    public int ToDepartmentId { get; set; }
    public Department? ToDepartment { get; set; }
    
    public int ReasonId { get; set; }
    public MovementReason? Reason { get; set; }
    
    public DateTime MovedAt { get; set; }
    public required string MovedBy { get; set; }
    public string? Note { get; set; }
}