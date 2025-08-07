namespace DataAccess.Models;

public class UserWithRolesDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = null!;
}