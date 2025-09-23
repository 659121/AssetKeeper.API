using CoreLogic.Models.DTO.Auth;

namespace CoreLogic;

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(RegisterDto regDto, CancellationToken cancellationToken = default);
    Task<LoginResult> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
}