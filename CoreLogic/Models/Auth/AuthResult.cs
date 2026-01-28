namespace CoreLogic.Models.Auth;
public record LoginResult(bool Success, string? Token = null, string? Message = null)
{
    public static LoginResult SuccessResult(string token, string message)
        => new(true, token, message);

    public static LoginResult FailResult(string message)
        => new(false, Message: message);
}
public record RegisterResult(bool Success, int? UserId = null, string? Message = null)
{
    public static RegisterResult SuccessResult(int userId, string? message = null)
        => new(true, userId, message);

    public static RegisterResult FailResult(string message)
        => new(false, Message: message);
}