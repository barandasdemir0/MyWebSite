using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.AuthDtos;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using SharedKernel.Enums;
using SharedKernel.Exceptions;

namespace BusinessLayer.Concrete;

public class TwoFactorManager : ITwoFactorService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;

    public TwoFactorManager(UserManager<AppUser> userManager, IEmailService emailService, ITokenService tokenService)
    {
        _userManager = userManager;
        _emailService = emailService;
        _tokenService = tokenService;
    }

    public async Task<bool> ConfirmAuthenticatorSetupAsync(string userId, string Code, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
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

    public async Task<bool> SendMailOtpAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user?.Email == null)
        {
            return false;
        }

        await _userManager.UpdateSecurityStampAsync(user);
        var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

        await _emailService.SendAsync(user.Email, "Giriş Doğrulama Kodunuz", $"Giriş kodunuz: <b>{code}</b><br/>Bu kod 5 dakika geçerlidir.", cancellationToken);
        return true;
    }

    public async Task<Setup2FAResultDto> SetupAuthenticatorAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Kullanıcı bulunamadı",userId);
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
            ManualEntryKey = key
        };
    }

    public async Task<bool> Toggle2FAAsync(string userId, Toggle2FADto toggle2FADto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;

        }
        await _userManager.SetTwoFactorEnabledAsync(user, toggle2FADto.Enable);
        user.Preferred2FAProvider = toggle2FADto.Enable ? toggle2FADto.Provider : TwoFactorProvider.None;
        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<LoginResultDto> VerifyTwoFactorAsync(TwoFactorVerifyDto dto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null)
        {
            return new LoginResultDto
            {
                Success = false,
                Error = "Kullanıcı Bulunamadı"
            };
        }

        var provider = dto.Provider switch
        {
            TwoFactorProvider.Authenticator => _userManager.Options.Tokens.AuthenticatorTokenProvider,
            TwoFactorProvider.Email => TokenOptions.DefaultEmailProvider,
            _ => TokenOptions.DefaultEmailProvider
        }; //_ discard patterndaıdr none dahil tümünü yakalar 

        var valid = await _userManager.VerifyTwoFactorTokenAsync(user, provider, dto.Code);
        if (!valid)
        {
            return new LoginResultDto
            {
                Success = false,
                Error= "Geçersiz veya süresi dolmuş kod"
            };
        }

        return new LoginResultDto
        {
            Success = true,
            Token = await _tokenService.CreateAccessTokenAsync(user),
            RefreshToken = await _tokenService.CreateRefreshTokenAsync(user, dto.DeviceInfo)

        };
    }

   



  
}
