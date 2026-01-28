namespace DataAccess.Models;
public class Department
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public required string Name { get; set; }
    public bool IsActive { get; set; } = true;
}