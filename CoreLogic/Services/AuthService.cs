using CoreLogic.Domain;
using CoreLogic.Interfaces;
using CoreLogic.Models.Auth;

namespace CoreLogic.Services;
internal class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IAuthRepository authRepository, ITokenService tokenService)
    {
        _authRepository = authRepository;
        _tokenService = tokenService;
    }

    public async Task<RegisterResult> RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default)
    {
        var existingUser = await _authRepository.GetUserByUsernameAsync(command.Username, cancellationToken);
        if (existingUser != null)
            return RegisterResult.FailResult("Пользователь с таким именем уже существует");

        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password, salt);

        var user = new User
        {
            Username = command.Username,
            Salt = salt,
            PasswordHash = passwordHash,
            IsActive = true,
            RegDate = DateTime.Now
        };

        await _authRepository.CreateUserAsync(user, cancellationToken);
        return RegisterResult.SuccessResult(user.Id, "Пользователь успешно зарегистрирован");
    }

    public async Task<LoginResult> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _authRepository.GetUserByUsernameAsync(command.Username, cancellationToken);

        if (user is null)
            return LoginResult.FailResult("Пользователь с таким именем не найден");

        if (!user.IsActive)
            return LoginResult.FailResult("Учетная запись заблокирована");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash))
            return LoginResult.FailResult("Неверный пароль");

        string? previousLogin = user.LastLogin?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;

        // Обновляем время последнего входа
        await _authRepository.UpdateUserLastLoginAsync(user.Id, DateTime.UtcNow, cancellationToken);

        // Генерируем токен на основе ДОМЕННОЙ сущности
        var token = _tokenService.GenerateJwtToken(user);

        return LoginResult.SuccessResult(token, previousLogin);
    }
}