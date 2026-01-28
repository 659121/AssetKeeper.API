namespace DataAccess.Models;

public class MovementReason
{
    public Guid Id { get; set; }
    public required string Code { get; set; }  // 'transfer', 'repair', 'return'
    public required string Name { get; set; }  // 'Передача', 'Ремонт', 'Возврат'
    public required string Description { get; set; }
    
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}