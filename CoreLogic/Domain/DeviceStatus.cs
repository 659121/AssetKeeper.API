namespace CoreLogic.Domain;

public class DeviceStatus
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;  // 'active', 'repair', 'disposed'
    public string Name { get; set; } = null!;  // 'В работе', 'В ремонте', 'Списано'
    
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}