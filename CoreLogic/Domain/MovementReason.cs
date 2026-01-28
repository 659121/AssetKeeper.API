namespace CoreLogic.Domain;

public class MovementReason
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;  // 'transfer', 'repair', 'return'
    public string Name { get; set; } = null!;  // 'Передача', 'Ремонт', 'Возврат'
    public string Description { get; set; } = null!;
    
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}