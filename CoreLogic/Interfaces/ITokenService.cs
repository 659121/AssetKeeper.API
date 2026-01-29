using CoreLogic.Domain;

namespace CoreLogic.Interfaces;
public interface ITokenService
{
    string GenerateJwtToken(User user);
    //TODO GenerateRefreshToken();
}