using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AuthDtos.Items;
using DtoLayer.AuthDtos.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Concrete;

public class TokenManager : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IRefreshTokenDal _refreshTokenDal;
    private readonly IConfiguration _configuration;

    public TokenManager(IRefreshTokenDal refreshTokenDal, IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _refreshTokenDal = refreshTokenDal;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<string> CreateAccessTokenAsync(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email??""),
            new Claim(ClaimTypes.Name,$"{user.Name} {user.Surname}")

        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));


        var token = new JwtSecurityToken
            (
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<LoginResultDto> RefreshTokenAsync(RefreshTokenRequestDto dto, string deviceInfo, CancellationToken cancellationToken)
    {
        ClaimsPrincipal? principal;
        try
        {
            principal = GetPrincipalFromExpiredToken(dto.AccessToken);
        }
        catch
        {
            return Fail("Geçersiz Token");
        }

        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Fail("Geçersiz Token");
        }
        var storedToken = await _refreshTokenDal.GetValidTokenAsync(dto.RefreshToken, Guid.Parse(userId), cancellationToken);
        if (storedToken == null)
        {
            return Fail("Refresh Token Geçersiz veya süresi dolmuş");
        }

        storedToken.IsRevoked = true;
        await _refreshTokenDal.SaveChangesAsync(cancellationToken);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Fail("Kullanıcı Bulunamadı");
        }

        var newAccessToken = await CreateAccessTokenAsync(user);

        var newRefreshToken = await CreateRefreshTokenAsync(user, deviceInfo, cancellationToken);

        return new LoginResultDto
        {
            Success = true,
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };


    }


    public async Task RevokeTokensAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.UpdateSecurityStampAsync(user);

            await _refreshTokenDal.RevokeAllByUserAsync(user.Id, cancellationToken);
        }
    }
    public async Task<string> CreateRefreshTokenAsync(AppUser user, string? deviceInfo, CancellationToken cancellation = default)
    {
        var tokenBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);

        var tokenString = Convert.ToBase64String(tokenBytes);

        var refreshToken = new RefreshToken
        {
            Token = tokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            DeviceInfo = deviceInfo,
            UserId = user.Id
        };

        await _refreshTokenDal.AddAsync(refreshToken, cancellation);
        return tokenString;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var key = new SymmetricSecurityKey
           (
               Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)
           );

        var validation = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = key,
            ValidateLifetime = false // süresi dolmuş token'ı da oku
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

    private static LoginResultDto Fail(string error)
    {
        return new LoginResultDto
        {
            Success = false,
            Error = error

        };
    }
}
