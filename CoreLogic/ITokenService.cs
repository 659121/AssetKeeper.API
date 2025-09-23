using DataAccess.Models;
using System.Security.Claims;

namespace CoreLogic;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    //TODO GenerateRefreshToken();
}
