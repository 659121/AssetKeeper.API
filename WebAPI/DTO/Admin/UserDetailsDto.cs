namespace CoreLogic.Models.DTO.Admin;
public record UserDetailsDto(
int Id,
string Username,
DateTime RegDate,
DateTime? LastLogin,
bool IsActive,
List<string> Roles);