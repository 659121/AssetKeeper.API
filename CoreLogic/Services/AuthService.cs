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

        // Создаём ДОМЕННУЮ сущность
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
        var userEntity = await _authRepository.GetUserByUsernameAsync(command.Username, cancellationToken);

        if (userEntity is null)
            return LoginResult.FailResult("Пользователь с таким именем не найден");

        if (!userEntity.IsActive)
            return LoginResult.FailResult("Учетная запись заблокирована");

        if (!BCrypt.Net.BCrypt.Verify(command.Password, userEntity.PasswordHash))
            return LoginResult.FailResult("Неверный пароль");

        string lastLogin = userEntity.LastLogin is not null
            ? userEntity.LastLogin.ToString()!
            : string.Empty;

        await _authRepository.UpdateUserLastLoginAsync(userEntity.Id, DateTime.UtcNow, cancellationToken);

        var token = _tokenService.GenerateJwtToken(userEntity);
        string? v = userEntity.LastLogin.ToString();
        return LoginResult.SuccessResult(token, lastLogin);
    }
}