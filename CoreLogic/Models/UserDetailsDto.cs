namespace CoreLogic.Models;
public record UserDetailsDto(
int Id,
string Username,
DateTime RegDate,
DateTime? LastLogin,
bool IsActive,
List<string> Roles);