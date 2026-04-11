using CV.EntityLayer.Entities;
using DtoLayer.AuthDtos.Items;
using DtoLayer.AuthDtos.Responses;
using System.Security.Claims;

namespace BusinessLayer.Abstract;

public interface ITokenService
{
    Task<string> CreateAccessTokenAsync(AppUser user);
    Task RevokeTokensAsync(string userId, CancellationToken cancellationToken);
    Task<LoginResultDto> RefreshTokenAsync(RefreshTokenRequestDto dto, string deviceInfo, CancellationToken cancellationToken);
    Task<string> CreateRefreshTokenAsync(AppUser user, string? deviceInfo, CancellationToken cancellation = default);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
