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
    private readonly IConfiguration _configuration;// bunun amacı dsyadaki secret key apsetting json gibi yerleride okumak

    public TokenManager(IRefreshTokenDal refreshTokenDal, IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _refreshTokenDal = refreshTokenDal;
        _configuration = configuration;
        _userManager = userManager;
    }

    // Access token oluşturma metodu
    public async Task<string> CreateAccessTokenAsync(AppUser user)
    {
        // Secret key'i al ve güvenlik anahtarı oluştur
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        // Kullanıcının rollerini al bu roller token içinde claim olarak taşınacak
        var roles = await _userManager.GetRolesAsync(user); // Token için gerekli claim'leri oluştur

        //claim : Token içinde taşınacak bilgileri temsil eder. Örneğin, kullanıcının kimliği, e-posta adresi veya rolleri gibi bilgiler claim olarak eklenebilir.
        var claims = new List<Claim> 
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()), // Kullanıcının benzersiz kimliğini temsil eder
            new Claim(ClaimTypes.Email,user.Email??""), // Kullanıcının e-posta adresini temsil eder
            new Claim(ClaimTypes.Name,$"{user.Name} {user.Surname}") // Kullanıcının tam adını temsil eder

        }; 
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));  // Kullanıcının rollerini claim olarak ekle

        // JWT token'ını oluştur
        var token = new JwtSecurityToken
            (
            issuer: _configuration["Jwt:Issuer"],// Token'ı veren taraf
            audience: _configuration["Jwt:Audience"], // Token'ı kullanacak taraf
            claims: claims, // Token içinde taşınacak bilgiler
            expires: DateTime.UtcNow.AddHours(1), // Token'ın geçerlilik süresi
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256) // Token'ı imzalamak için kullanılan güvenlik anahtarı ve algoritma
            );

        return new JwtSecurityTokenHandler().WriteToken(token);// Token'ı string formatında döndür anlamadım burayı anlat : JwtSecurityTokenHandler, JWT token'larını oluşturmak ve doğrulamak için kullanılan bir sınıftır. WriteToken metodu, oluşturulan JwtSecurityToken nesnesini alır ve onu string formatında bir JWT token'ına dönüştürür. Bu string formatındaki token, istemcilere gönderilmek üzere kullanılabilir.
    }


    // Refresh token yenileme metodu
    public async Task<LoginResultDto> RefreshTokenAsync(RefreshTokenRequestDto dto, string deviceInfo, CancellationToken cancellationToken)
    {
        ClaimsPrincipal? principal;  // claimsprincipal, token içindeki claim'leri temsil eder. Bu, token'ın geçerliliğini doğrulamak ve içindeki bilgileri almak için kullanılır.
        try
        {
            //try amacımız token geçersizse veya süresi dolmuşsa oluşacak hatayı yakalamak ve uygun bir hata mesajı döndürmek
            principal = GetPrincipalFromExpiredToken(dto.AccessToken);  // Access token'ın süresi dolmuş olsa bile içindeki claim'leri al
        }
        catch // Token geçersizse veya başka bir hata oluşursa
        {
            return Fail("Geçersiz Token"); 
        }
        // Token'dan kullanıcı kimliğini al
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier); 
        if (userId == null) 
        {
            return Fail("Geçersiz Token");
        }
        // Veritabanında bu refresh token'ı ve kullanıcıya ait geçerli bir token olup olmadığını kontrol et
        var storedToken = await _refreshTokenDal.GetValidTokenAsync(dto.RefreshToken, Guid.Parse(userId), cancellationToken);
        //userıd 'yi guid'e çevirdik çünkü veritabanında öyle tutuluyor ama token içinde string olarak var
        if (storedToken == null)
        {
            return Fail("Refresh Token Geçersiz veya süresi dolmuş");
        }


        // Eski refresh token'ı geçersiz kıl
        storedToken.IsRevoked = true; 
        await _refreshTokenDal.SaveChangesAsync(cancellationToken);

        // Kullanıcıyı bul
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Fail("Kullanıcı Bulunamadı");
        } 

        var newAccessToken = await CreateAccessTokenAsync(user); // Yeni access token oluştur

        var newRefreshToken = await CreateRefreshTokenAsync(user, deviceInfo, cancellationToken); // Yeni refresh token oluştur

        // Yeni token'ları döndür
        return new LoginResultDto 
        {
            Success = true, // Token yenileme işlemi başarılı
            Token = newAccessToken, // Yeni access token
            RefreshToken = newRefreshToken // Yeni refresh token
        };


    }

   
    public async Task RevokeTokensAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.UpdateSecurityStampAsync(user);  // Kullanıcının security stamp'ini güncellemek, mevcut tüm token'ların geçersiz olmasını sağlar

            await _refreshTokenDal.RevokeAllByUserAsync(user.Id, cancellationToken);  // Veritabanındaki tüm refresh token'ları geçersiz kıl
        }
    }
    public async Task<string> CreateRefreshTokenAsync(AppUser user, string? deviceInfo, CancellationToken cancellation = default)
    {
        var tokenBytes = new byte[64];  // Güçlü bir refresh token oluşturmak için rastgele byte'lar kullanıyoruz
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);  // Rastgele byte'ları doldur

        var tokenString = Convert.ToBase64String(tokenBytes);  // Byte'ları base64 string formatına dönüştürerek token'ı oluştur
        var refreshToken = new RefreshToken  
        {
            Token = tokenString,  // Oluşturulan refresh token string'i
            ExpiresAt = DateTime.UtcNow.AddDays(7),  // Refresh token'ın geçerlilik süresi (örneğin, 7 gün)
            DeviceInfo = deviceInfo,  // Refresh token'ın hangi cihazda kullanıldığını belirtmek için opsiyonel bir alan
            UserId = user.Id  // Refresh token'ın hangi kullanıcıya ait olduğunu belirtmek için kullanıcı ID'si
        };

        await _refreshTokenDal.AddAsync(refreshToken, cancellation); // Oluşturulan refresh token'ı veritabanına kaydet
        return tokenString;// Oluşturulan refresh token'ı string formatında döndür
    }


    // Süresi dolmuş bir token'dan claim'leri almak için yardımcı metot
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
  
    {
        var key = new SymmetricSecurityKey 
           (
               Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!) // Secret key'i al ve güvenlik anahtarı oluştur
           );

        var validation = new TokenValidationParameters  // Token doğrulama parametrelerini belirle
        {
            ValidateIssuer = true, // Token'ı veren tarafın doğrulanması
            ValidateAudience = true,// Token'ı kullanacak tarafın doğrulanması
            ValidIssuer = _configuration["Jwt:Issuer"], // Token'ı veren tarafın beklenen değeri
            ValidAudience = _configuration["Jwt:Audience"],// Token'ı kullanacak tarafın beklenen değeri
            IssuerSigningKey = key,// Token'ı imzalamak için kullanılan güvenlik anahtarı
            ValidateLifetime = false // Token'ın süresinin doğrulanmaması, çünkü süresi dolmuş token'ları da işlemek istiyoruz
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _); //token doğrulama işlemi gerçekleştirilir ve token içindeki claim'ler alınır. ValidateToken metodu, token'ı doğrular ve geçerliyse token içindeki claim'leri içeren bir ClaimsPrincipal nesnesi döndürür. Eğer token geçersizse veya doğrulama başarısız olursa, bu metot bir istisna fırlatır. out _ ifadesi, ValidateToken metodunun üçüncü parametresi olan SecurityToken nesnesini kullanmadığımızı belirtmek için kullanılır. Bu, kodun daha temiz ve anlaşılır olmasını sağlar.
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
