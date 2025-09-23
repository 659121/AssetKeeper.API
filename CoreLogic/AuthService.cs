using CoreLogic.Models.DTO.Auth;
using DataAccess;
using DataAccess.Models;

namespace CoreLogic;
internal class AuthService(IAuthRepository authRepository, ITokenService tokenService) : IAuthService
{
    public async Task<RegisterResult> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        //TODO валидация реги
        var existingUser = await authRepository.GetUserByUsernameAsync(registerDto.Username, cancellationToken);
        if (existingUser != null)
            return RegisterResult.FailResult("Пользователь с таким именем уже существует");

        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, salt);

        var user = new User
        {
            Username = registerDto.Username,
            Salt = salt,
            PasswordHash = passwordHash,
            IsActive = true,
            RegDate = DateTime.Now
        };

        await authRepository.CreateUserAsync(user, cancellationToken);
        return RegisterResult.SuccessResult(user.Id, "Пользователь успешно зарегистрирован");
    }

    public async Task<LoginResult> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var userEntity = await authRepository.GetUserByUsernameAsync(loginDto.Username, cancellationToken);

        if (userEntity is null)
            return LoginResult.FailResult("Пользователь с таким именем не найден");

        if (!userEntity.IsActive)
            return LoginResult.FailResult("Учетная запись заблокирована");

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, userEntity.PasswordHash))
            return LoginResult.FailResult("Неверный пароль");

        string lastLogin = userEntity.LastLogin is not null
            ? userEntity.LastLogin.ToString()!
            : string.Empty;

        await authRepository.UpdateUserLastLoginAsync(userEntity.Id, DateTime.UtcNow, cancellationToken);

        var token = tokenService.GenerateJwtToken(userEntity);
        string? v = userEntity.LastLogin.ToString();
        return LoginResult.SuccessResult(token, lastLogin);
    }
}