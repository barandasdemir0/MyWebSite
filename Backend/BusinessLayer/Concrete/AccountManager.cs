using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Enums;

namespace BusinessLayer.Concrete;

public class AccountManager:IAccountService
{

    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IPasswordResetTokenDal _passwordResetTokenDal;

    public AccountManager(UserManager<AppUser> userManager, IEmailService emailService, IPasswordResetTokenDal passwordResetTokenDal)
    {
        _userManager = userManager;
        _emailService = emailService;
        _passwordResetTokenDal = passwordResetTokenDal;
    }

    public async Task ForgotPasswordAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email); //eğer şifre unutulduysa kullanıcıyı ara
        if (user == null) //eğer kullanıcı yoksa
        {
            return; //kullanıcı bulunamadıysa sonlandır metottan çık böylelikle hacker sistemde kullanıcı varmı yokmu bulamaz
        }

        await _userManager.UpdateSecurityStampAsync(user); // kullanıcı şifresini unuttuğunda eski tokenleri geçersiz kullarız
        var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider); // Kullanıcı için tek seferlik doğrulama kodu (OTP) ÜRETİR


        await _emailService.SendAsync(email, "Şifre Sıfırlama",
       $"Şifre sıfırlama kodunuz: <b>{code}</b><br/>Bu kod 10 dakika geçerlidir.", cancellationToken); // Üretilen OTP kodunu kullanıcının e-posta adresine GÖNDERİR.


    }



    public async Task<string?> VerifyResetOtpAsync(string email, string code, TwoFactorProvider provider, CancellationToken cancellationToken) //şifremi unuttumdan gelen kodu doğrulayıp geçici ekrana göndermek 
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;// burada   Task<string?> kullandığımızdan dolayı null zorunludur 
        }//kullanıcının varlığı teyit ediliyor 

        var tokenProvider = provider switch
        { 
            //KLASİK İF ELSE  MANTIĞININ YERİNE SWİTCH MANTIĞI KULLANIMI 
            TwoFactorProvider.Authenticator => _userManager.Options.Tokens.AuthenticatorTokenProvider, //if yani eğer autendicator seçiliyse authenticator ila doğrula
            TwoFactorProvider.Email => TokenOptions.DefaultEmailProvider, // email seçiliyse email doğrula 
            _ => TokenOptions.DefaultEmailProvider // _ alt tire varsayılan olarak emaili seç manasındadır 
        }; //kullanıcı neyi seçmiş hangi doğrulama ile giiş yapacak

        var valid = await _userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, code); //daha önce girilen kod ile kullanıcının girdiği kodu larşılaştır

        if (!valid) //eğer kodun süresi dolduysa false dön null dön 
        {
            return null;
        }

        var resetToken = new PasswordResetToken //eğer null değilse reset token oluştur
        {
            Email = email, //tokenin kime ait olduğu
            ExpiresAt = DateTime.UtcNow.AddMinutes(3)//tokeini süersi
        };

        await _passwordResetTokenDal.AddAsync(resetToken, cancellationToken);//tokeni veritabanına kaydet 
        return resetToken.Token; //tokeni döndür

    }

    public async Task<bool> SetNewPasswordAsync(string email, string newPassword, string resetToken, CancellationToken cancellationToken)
    { // yeni şifre atama metotu

        var tokenEntity = await _passwordResetTokenDal.GetValidTokenAsync(resetToken, email, cancellationToken); //reset password daki token kontrolü yapıyor
        if (tokenEntity == null) //token varmı
        {
            return false;
        }

        await _passwordResetTokenDal.MarkAsUsedAsync(tokenEntity, cancellationToken); //burası token görülmüşmü tıklanılmışmı tıklanıldıysa iptal etme yani tek kullanımlık


        //kullanıcıyı buk
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }


        //token üretir şifre sıfırlama anahtarı üretir
        var identityResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, identityResetToken, newPassword);//yeni şifreyi belirler kontrolleri yapar
        return result.Succeeded; // işlemin başarılı olup olmadığını anlamak için true false döner

    }
}
