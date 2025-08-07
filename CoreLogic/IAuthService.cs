using CoreLogic.Models.DTO.Auth;

namespace CoreLogic;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto regDto, CancellationToken cancellationToken = default);
    Task<AuthResult> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
}