namespace CoreLogic.Domain;
public class DeviceStatus
{
    public int Id { get; set; }
    public required string Code { get; set; }  // 'active', 'repair', 'disposed'
    public required string Name { get; set; }  // 'В работе', 'В ремонте', 'Списано'
    
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}