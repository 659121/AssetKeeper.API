namespace CoreLogic.Models.Auth;

public record LoginCommand
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}