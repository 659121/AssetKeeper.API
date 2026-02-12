namespace CoreLogic.Models;

public class DeviceMovementDto
{
    public Guid Id { get; set; }
    public DateTime MovedAt { get; set; }
    public string? FromDepartmentName { get; set; }
    public string ToDepartmentName { get; set; } = null!;
    public string ReasonName { get; set; } = null!;
    public string MovedBy { get; set; } = null!;
    public string? Note { get; set; }
    public int? OldSticker { get; set; }
    public int? NewSticker { get; set; }
}