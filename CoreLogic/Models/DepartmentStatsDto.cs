namespace CoreLogic.Models;

public class DepartmentStatsDto
{
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public int DeviceCount { get; set; }
    public int ActiveCount { get; set; }
    public int RepairCount { get; set; }
}