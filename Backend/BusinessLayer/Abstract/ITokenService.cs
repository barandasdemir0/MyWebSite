using EntityLayer.Entities;
using System.Security.Claims;

namespace BusinessLayer.Abstract;

public interface ITokenService
{
    Task<string> CreateAccessTokenAsync(AppUser user);
    Task<string> CreateRefreshTokenAsync(AppUser user, string? deviceInfo, CancellationToken cancellation = default);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
