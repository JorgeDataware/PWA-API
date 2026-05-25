using PWA_API.Domain.Entities;

namespace PWA_API.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    int GetExpiresIn();
}
