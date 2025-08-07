namespace CoreLogic.Models.DTO.Auth;

public class AuthResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Error { get; set; }

    public static AuthResult SuccessResult(string token)
        => new AuthResult { Success = true, Token = token};

    public static AuthResult FailResult(string error)
        => new AuthResult { Success = false, Error = error };
}