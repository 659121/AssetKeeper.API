namespace CoreLogic.Models.Admin;
public class UpdateUserCommand
{
    public bool? IsActive { get; init; }
    public List<string>? Roles { get; init; }
}