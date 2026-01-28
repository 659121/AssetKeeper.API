namespace CoreLogic.Models;
public class UpdateUserRequest
{
    public bool? IsActive { get; set; } = null;
    public List<string>? Roles { get; set; } = null;
}