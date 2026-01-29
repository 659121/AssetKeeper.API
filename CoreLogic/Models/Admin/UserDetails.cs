namespace CoreLogic.Models.Admin;
public record UserDetails(
int Id,
string Username,
DateTime RegDate,
DateTime? LastLogin,
bool IsActive,
List<string> Roles);