using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.AuthDtos.Items;
using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using System.Security.Claims;

namespace BusinessLayer.Concrete;

public class AuthManager : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger<AuthManager> _logger;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;


    public AuthManager(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, ILogger<AuthManager> logger, ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _tokenService = tokenService;
        _mapper = mapper;
    }



    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken)//kullanıcıdan bir çok veri alacağız login işlemi olacak 
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email); //kullanıcıyı bul
        

        if (user==null) //kullanıcı null sistemde yok
        {
            _logger.LogWarning("Başarısız giriş denemesi - kullanıcı yok: {Email}", loginDto.Email);
            return Fail("Geçersiz Eposta veya şifre"); // kullanıcı sistemde varmı yokmu anlamamadı adına geçersiz diyoruz 
        }

        if (!user.IsApproved) //şifren doğru olsa bile onayıma sunduruyorum böylelikle sorun yaşatmasın 
        {
            _logger.LogWarning("Onaylanmamış Hesap Giriş Denemesi:{Email}", loginDto.Email);
            return Fail("Hesabınız Henüz Admin Tarafından Onaylanmadı");
        }

        if (await _userManager.IsLockedOutAsync(user)) // hesap kilitlimi bruteforce koruması yapıyorum 
        {
            _logger.LogWarning("Kilitli hesaba giriş denemesi: {Email}", loginDto.Email);
            return Fail("Hesabınız Geçiçi Olarak Kilitlendi");
        }
        // Not: CheckPasswordAsync kullanıldığı için RequireConfirmedEmail bypass edilmiş.
        // EmailConfirmed kontrolü onay sistemi tarafından yönetiliyor (ApproveUserAsync → EmailConfirmed = true).
        // IsApproved kontrolü yukarıda yapıldığı için onaylanmamış kullanıcı buraya ulaşamaz.

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) //şifre ve kullanıcı adını yapıyorum
        {
            await _userManager.AccessFailedAsync(user); //eğer şifre yanlışsa hakkını bir azalt
            _logger.LogWarning("Başarısız giriş denemesi - yanlış şifre: {Email}", loginDto.Email);
            return Fail("Geçersiz e-posta veya şifre"); //ve mesajı ver
        }

        await _userManager.ResetAccessFailedCountAsync(user); //eğer şifre doğruysa hataları girişleri sıfırlıyoruz seni affettik gibi 

      
        //2fa kontrolü
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
        var accessToken = await _tokenService.CreateAccessTokenAsync(user);//access token verilir yani kısa süreli giriş bileti 

        //refresh token   burada deviceinfo gönderiyoruz çünkü kullanıcı hangi ortamdan giriş yaptı böylelikle aktif 2 refresh tokeni tutuyoruz refresh token uzun süreli tokenelrdir 
        var refreshToken = await _tokenService.CreateRefreshTokenAsync(user, loginDto.DeviceInfo,cancellationToken);
        _logger.LogInformation("Kullanıcı giriş yaptı: {UserId}", user.Id);
        return new LoginResultDto
        {
            Success = true,
            Token = accessToken,
            RefreshToken = refreshToken
        };

    }

    public async Task RevokeTokensAsync(string userId, CancellationToken cancellationToken) //iptal kodu bu yani logout
    {
        await _tokenService.RevokeTokensAsync(userId, cancellationToken);
    } 


    //kayıt ol fonksiyonu
    public async Task<RegisterResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<AppUser>(registerDto); //register dtoyu manuel aktarmamak için yapılır 
        user.EmailConfirmed = false; //burada kullanıcı admin tarafından onaylanmadığı sürece sen üye değilsin 

        var result = await _userManager.CreateAsync(user, registerDto.Password); //kullanıcı oluşturur
        if (!result.Succeeded)//eğer hata bulursa
        {
            return new RegisterResultDto
            {
                Success = false,  //başarısız
                Errors = result.Errors.Select(x => x.Description).ToList()//neden başarısız açıklar
            };

        }
        //rol atama kısmı 
        if (!await _roleManager.RoleExistsAsync(RoleConsts.User))//yeni üye olan herkesi user yapar böylelikle admin yetkisi olmaz
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = RoleConsts.User
            });
        }
        await _userManager.AddToRoleAsync(user, RoleConsts.User);//kuullanıcıya userı atadı 

        _logger.LogInformation("Yeni kullanıcı kaydı: {Email}", registerDto.Email);
        return new RegisterResultDto
        {
            Success = true //sisteme yeni üye geldi mesajı düşer
        };
    }


 
    //tekrarı önlemek için azılan metottur
    
    private static LoginResultDto Fail(string error)
    {
        return new LoginResultDto
        {
            Success = false, //truemu falsemı
            Error = error //nedeni

        };
    }

   
}
