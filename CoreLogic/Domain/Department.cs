namespace CoreLogic.Domain;

public class Department
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}