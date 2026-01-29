namespace WebAPI.DTO.Admin;
public class UpdateUserRequest
{
    public bool? IsActive { get; init; }
    public List<string>? Roles { get; init; }
}