namespace CoreLogic.Domain;

public class DeviceMovement
{
    public Guid Id { get; set; }
    public int DeviceId { get; set; }
    
    public int? FromDepartmentId { get; set; }
    
    public int ToDepartmentId { get; set; }
    
    public int ReasonId { get; set; }
    
    public DateTime MovedAt { get; set; }
    public string MovedBy { get; set; } = null!;
    public string? Note { get; set; }
}