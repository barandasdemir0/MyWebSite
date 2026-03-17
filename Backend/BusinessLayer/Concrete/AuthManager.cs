using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Concrete;

public class AuthManager : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthManager> _logger;
    private readonly IRefreshTokenDal _refreshTokenDal;


    public AuthManager(UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<IdentityRole<Guid>> roleManager, ILogger<AuthManager> logger, IRefreshTokenDal refreshTokenDal, IEmailService emailService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _logger = logger;
        _refreshTokenDal = refreshTokenDal;
        _emailService = emailService;
    }



    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        

        if (user==null)
        {
            _logger.LogWarning("Başarısız giriş denemesi - kullanıcı yok: {Email}", loginDto.Email);
            return Fail("Geçersiz Eposta veya şifre");
        }

        if (!user.IsApproved)
        {
            _logger.LogWarning("Onaylanmamış Hesap Giriş Denemesi:{Email}", loginDto.Email);
            return Fail("Hesabınız Henüz Admin Tarafından Onaylanmadı");
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("Kilitli hesaba giriş denemesi: {Email}", loginDto.Email);
            return Fail("Hesabınız Geçiçi Olarak Kilitlendi");
        }
        // Not: CheckPasswordAsync kullanıldığı için RequireConfirmedEmail bypass edilmiş.
        // EmailConfirmed kontrolü onay sistemi tarafından yönetiliyor (ApproveUserAsync → EmailConfirmed = true).
        // IsApproved kontrolü yukarıda yapıldığı için onaylanmamış kullanıcı buraya ulaşamaz.

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            await _userManager.AccessFailedAsync(user);
            _logger.LogWarning("Başarısız giriş denemesi - yanlış şifre: {Email}", loginDto.Email);
            return Fail("Geçersiz e-posta veya şifre");
        }

        await _userManager.ResetAccessFailedCountAsync(user);

      

        if (await _userManager.GetTwoFactorEnabledAsync(user))
        {
          
            _logger.LogInformation("2FA doğrulama bekleniyor: {UserId}", user.Id);
            //2fa açıksa doğrulama ekranına gönder
            return new LoginResultDto
            {
                Success = true,
                RequiresTwoFactor = true,
                UserId = user.Id.ToString()

            };
        }

     
       


        //direkt token ver



        var accessToken = await CreateJwtAsync(user);
        var refreshToken = await CreateRefreshTokenAsync(user, loginDto.DeviceInfo,cancellationToken);
        _logger.LogInformation("Kullanıcı giriş yaptı: {UserId}", user.Id);
        return new LoginResultDto
        {
            Success = true,
            Token = accessToken,
            RefreshToken = refreshToken
        };

    }

    public async Task RevokeTokensAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user!=null)
        {
            await _userManager.UpdateSecurityStampAsync(user);

            await _refreshTokenDal.RevokeAllByUserAsync(user.Id, cancellationToken);
        }
    } 

    public async Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return new RegisterResultDto
            {
                Success = false,
                Errors = result.Errors.Select(x => x.Description).ToList()
            };

        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = "User"
            });
        }
        await _userManager.AddToRoleAsync(user, "User");

        _logger.LogInformation("Yeni kullanıcı kaydı: {Email}", registerDto.Email);
        return new RegisterResultDto
        {
            Success = true
        };
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
        if (storedToken==null)
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

        var newAccessToken = await CreateJwtAsync(user);

        var newRefreshToken = await CreateRefreshTokenAsync(user, deviceInfo, cancellationToken);

        return new LoginResultDto
        {
            Success = true,
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };


    }

    private async Task<string> CreateRefreshTokenAsync(AppUser user, string? deviceInfo, CancellationToken cancellationToken)
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
            userId = user.Id
        };

        await _refreshTokenDal.AddAsync(refreshToken, cancellationToken);
        return tokenString;


    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
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
            ValidateLifetime = false // süresi dolmuş tokenıda oıku
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }
    private async Task<string> CreateJwtAsync(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email??""),
            new Claim(ClaimTypes.Name,$"{user.Name}{user.Surname}")

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
    private static LoginResultDto Fail(string error)
    {
        return new LoginResultDto
        {
            Success = false,
            Error = error

        };
    }

    public async Task ForgotPasswordAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user==null)
        {
            return;
        }

        await _userManager.UpdateSecurityStampAsync(user);
        var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);


        await _emailService.SendAsync(email, "Şifre Sıfırlama",
       $"Şifre sıfırlama kodunuz: <b>{code}</b><br/>Bu kod 10 dakika geçerlidir.", cancellationToken);


    }

   

    public async Task<bool> VerifyResetOtpAsync(string email, string code, TwoFactorProvider provider, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user ==null)
        {
            return false;
        }

        var tokenProvider = provider switch
        {
            TwoFactorProvider.Authenticator => _userManager.Options.Tokens.AuthenticatorTokenProvider,
            TwoFactorProvider.Email => TokenOptions.DefaultEmailProvider,
            _ => TokenOptions.DefaultEmailProvider
        };

        return await _userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, code );
    }

    public async Task<bool> SetNewPasswordAsync(string email, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user==null)
        {
            return false;
        }



        await _userManager.RemovePasswordAsync(user);
        var result = await _userManager.AddPasswordAsync(user, newPassword);
        return result.Succeeded;
    }
}
