using CoreLogic.Models.Auth;

namespace CoreLogic.Interfaces;

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default);
    Task<LoginResult> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);
}