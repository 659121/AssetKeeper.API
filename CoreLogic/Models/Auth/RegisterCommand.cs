namespace CoreLogic.Models.Auth;

public record RegisterCommand
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}