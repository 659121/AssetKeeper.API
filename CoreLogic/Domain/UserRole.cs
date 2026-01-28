namespace CoreLogic.Domain;

public class UserRole
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}