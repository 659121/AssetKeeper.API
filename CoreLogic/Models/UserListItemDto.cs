namespace CoreLogic.Models;
public record UserListItemDto(
int Id,
string Username,
DateTime RegDate,
DateTime? LastLogin,
bool IsActive,
List<string> Roles);