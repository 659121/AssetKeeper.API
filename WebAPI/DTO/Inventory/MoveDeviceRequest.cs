namespace WebAPI.DTO.Inventory;

public class MoveDeviceRequest
{
    public Guid DeviceId { get; set; }
    public Guid ToDepartmentId { get; set; }
    public Guid ReasonId { get; set; }
    public int? NewSticker { get; set; }
    public string? Note { get; set; }
}