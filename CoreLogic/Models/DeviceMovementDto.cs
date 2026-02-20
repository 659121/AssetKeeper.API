namespace CoreLogic.Models;

public class DeviceMovementDto
{
    public Guid Id { get; set; }
    public DateTime MovedAt { get; set; }
    public string? FromDepartmentName { get; set; }
    public string ToDepartmentName { get; set; } = null!;
    public string? ReasonCode { get; set; }
    public string ReasonName { get; set; } = null!;
    public string MovedBy { get; set; } = null!;
    public string? Note { get; set; }
    public string? OldSticker { get; set; }
    public string? NewSticker { get; set; }
}