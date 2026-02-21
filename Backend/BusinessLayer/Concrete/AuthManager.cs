using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QRCoder;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Concrete;

internal class AuthManager : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public AuthManager(UserManager<AppUser> userManager, IConfiguration configuration, IEmailService emailService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<bool> ConfirmAuthenticatorSetupAsync(string userId, string Code, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user==null)
        {
            return false;
        }
        var valid = await _userManager.VerifyTwoFactorTokenAsync(
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, Code
            );
        if (!valid)
        {
            return false;
        }
        await _userManager.SetTwoFactorEnabledAsync(user, true);
        user.Preferred2FAProvider = TwoFactorProvider.Authenticator;
        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user== null|| !await _userManager.CheckPasswordAsync(user,loginDto.Password))
        {
            return Fail("Geçersiz Eposta veya şifre");
        }
        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {
            return new LoginResultDto
            {
                Success = true,
                Token = CreateJwt(user)
            };
        }

        return new LoginResultDto
        {
            Success = true,
            RequiresTwoFactor = true,
            UserId = user.Id.ToString()
        };
    }

    public async Task RevokeTokensAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user!=null)
        {
            await _userManager.UpdateSecurityStampAsync(user);
        }
    }

    public async Task<bool> SendMailOtpAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user?.Email== null)
        {
            return false;
        }

        var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

        await _emailService.SendAsync(user.Email, "Giriş Doğrulama Kodunuz", $"Giriş kodunuz: <b>{code}</b><br/>Bu kod 5 dakika geçerlidir.", cancellationToken);
        return true;
    }

    public async Task<Setup2FAResultDto> SetupAuthenticatorAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user== null)
        {
            throw new Exception("Kullanıcı bulunamadı");
            //return Fail("Kullanıcı Bulunamadı");
        }
        await _userManager.ResetAuthenticatorKeyAsync(user);
        var key = await _userManager.GetAuthenticatorKeyAsync(user);

        var otpUri = $"otpauth://totp/{Uri.EscapeDataString("barandasdemir.com")}:" +
            $"{Uri.EscapeDataString(user.Email!)}?secret={key}&issuer=" +
            Uri.EscapeDataString("barandasdemir.com");


        using var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrData);
        var qrBytes = qrCode.GetGraphic(5);
        var base64 = Convert.ToBase64String(qrBytes);

        return new Setup2FAResultDto
        {
            QrCodeImageBase64 = $"data:image/png;base64,{base64}",
            ManuelEntryKey = key
        };
    }

    public async Task<LoginResultDto> VerifyTwoFactorAsync(TwoFactorVerifyDto dto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null)
        {
            return Fail("Kullanıcı Bulunamadı");
        }

        var provider = user.Preferred2FAProvider switch
        {
            TwoFactorProvider.Authenticator => _userManager.Options.Tokens.AuthenticatorTokenProvider,
            TwoFactorProvider.Email => TokenOptions.DefaultEmailProvider, _=>   TokenOptions.DefaultEmailProvider
        }; //_ discard patterndaıdr none dahil tümünü yakalar 

        var valid = await _userManager.VerifyTwoFactorTokenAsync(user, provider, dto.Code);
        if (!valid)
        {
            return Fail("Geçersiz veya süresi dolmuş kod");
        }

        return new LoginResultDto
        {
            Success = true,
            Token = CreateJwt(user)
        };
    }



    private string CreateJwt(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email??""),
            new Claim(ClaimTypes.Name,user.DisplayName ?? user.UserName ??""),
            new Claim(ClaimTypes.Role,"Admin")
        };

        var token = new JwtSecurityToken
            (
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
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
}
