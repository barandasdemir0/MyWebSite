using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BusinessLayer.Concrete;

public class AuthManager : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthManager> _logger;
    private readonly IRefreshTokenDal _refreshTokenDal;
    private readonly ITokenService _tokenService;
    private readonly IPasswordResetTokenDal _passwordResetTokenDal;
    private readonly IMapper _mapper;


    public AuthManager(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, ILogger<AuthManager> logger, IRefreshTokenDal refreshTokenDal, IEmailService emailService, ITokenService tokenService, IMapper mapper, IPasswordResetTokenDal passwordResetTokenDal)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _refreshTokenDal = refreshTokenDal;
        _emailService = emailService;
        _tokenService = tokenService;
        _mapper = mapper;
        _passwordResetTokenDal = passwordResetTokenDal;
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



        var accessToken = await _tokenService.CreateAccessTokenAsync(user);
        var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, loginDto.DeviceInfo,cancellationToken);
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
        var user = _mapper.Map<AppUser>(registerDto);
        user.EmailConfirmed = false;

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return new RegisterResultDto
            {
                Success = false,
                Errors = result.Errors.Select(x => x.Description).ToList()
            };

        }

        if (!await _roleManager.RoleExistsAsync(RoleConsts.User))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = RoleConsts.User
            });
        }
        await _userManager.AddToRoleAsync(user, RoleConsts.User);

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
            principal = _tokenService.GetPrincipalFromExpiredToken(dto.AccessToken);
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

        var newAccessToken = await _tokenService.CreateAccessTokenAsync(user);

        var newRefreshToken = await _tokenService.CreateRefreshTokenAsync(user, deviceInfo, cancellationToken);

        return new LoginResultDto
        {
            Success = true,
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };


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

   

    public async Task<string?> VerifyResetOtpAsync(string email, string code, TwoFactorProvider provider, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user ==null)
        {
            return null;
        }

        var tokenProvider = provider switch
        {
            TwoFactorProvider.Authenticator => _userManager.Options.Tokens.AuthenticatorTokenProvider,
            TwoFactorProvider.Email => TokenOptions.DefaultEmailProvider,
            _ => TokenOptions.DefaultEmailProvider
        };

        var valid = await _userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, code);

        if (!valid)
        {
            return null;
        }

        var resetToken = new PasswordResetToken
        {
            Email = email,
            ExpiresAt = DateTime.UtcNow.AddMinutes(3)
        };

        await _passwordResetTokenDal.AddAsync(resetToken, cancellationToken);
        return resetToken.Token;
        
    }

    public async Task<bool> SetNewPasswordAsync(string email, string newPassword, string resetToken, CancellationToken cancellationToken)
    {

        var tokenEntity = await _passwordResetTokenDal.GetValidTokenAsync(resetToken, email, cancellationToken);
        if (tokenEntity==null)
        {
            return false;
        }

        await _passwordResetTokenDal.MarkAsUsedAsync(tokenEntity, cancellationToken);



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
