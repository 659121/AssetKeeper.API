using CoreLogic.Models.DTO.Auth;
using DataAccess;
using DataAccess.Models;

namespace CoreLogic;
internal class AuthService(IAuthRepository authRepository, ITokenService tokenService) : IAuthService
{
    public async Task RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, salt);

        // TODO убрать модель бд, перевести на дто
        var user = new User
        {
            Username = registerDto.Username,
            Salt = salt,
            PasswordHash = passwordHash
        };

        await authRepository.CreateUserAsync(user, cancellationToken);
    }

    public async Task<AuthResult> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await authRepository.GetUserByUsernameAsync(loginDto.Username, cancellationToken);
        if (user is null)
            return AuthResult.FailResult("Пользователь не найден");

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            return AuthResult.FailResult("Неверный пароль");

        var token = tokenService.GenerateJwtToken(user);

        return AuthResult.SuccessResult(token);
    }
}