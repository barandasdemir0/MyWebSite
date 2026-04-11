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

    public async Task<string> CreateAccessTokenAsync(AppUser user)//kısa süreli token ürettirmemize yarayan metot
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)); //token oluştururken kullanacağımız gizli anahtar. bu anahtar token'ın doğrulanması için kullanılır. token oluşturulurken bu anahtar ile imzalanır ve token doğrulanırken de bu anahtar kullanılarak token'ın geçerliliği kontrol edilir.
        var roles = await _userManager.GetRolesAsync(user); //claim bilgileri hazırlanır claim nedir? token içine kullanıcıyla ilgili bilgilerin konulmasıdır. claimler sayesinde token içinde kullanıcının id'si, emaili, rolleri gibi bilgileri saklayabiliriz. bu bilgiler token doğrulandığında okunabilir ve uygulama içinde kullanılabilir hale gelir.
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email??""),
            new Claim(ClaimTypes.Name,$"{user.Name} {user.Surname}")

        }; //claim listesi oluşturulur ve içine kullanıcının id'si, emaili ve adı soyadı gibi bilgiler eklenir. bu bilgiler token içinde saklanır ve token doğrulandığında okunabilir hale gelir.
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r))); // kullanıcının rolleri de claim olarak eklenir. böylece token doğrulandığında kullanıcının hangi rollere sahip olduğu da bilinebilir.


        var token = new JwtSecurityToken //token oluşturulur. token oluşturulurken issuer, audience, claims, expiration ve signing credentials gibi bilgiler verilir. issuer token'ı oluşturan tarafı, audience token'ın hedef kitlesini, claims token içinde saklanacak bilgileri, expires token'ın geçerlilik süresini ve signing credentials token'ın imzalanması için kullanılacak anahtar ve algoritmayı belirtir.
            (
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256) 
            );

        return new JwtSecurityTokenHandler().WriteToken(token); //oluşturulan token'ı string olarak döndürür. bu token daha sonra client'a gönderilir ve client bu token'ı kullanarak uygulamaya erişim sağlar. token doğrulandığında token içindeki claimler okunarak kullanıcının kim olduğu ve hangi rollere sahip olduğu bilgisi elde edilir.
    }


    //refresh token'ı doğrulamak ve yeni bir access token oluşturmak için kullanılan metot. refresh token, access token'ın süresi dolduğunda yeni bir access token almak için kullanılır. refresh token daha uzun süre geçerlidir ve genellikle client tarafında saklanır. client, access token'ın süresi dolduğunda refresh token'ı kullanarak yeni bir access token talep eder. bu metot, gelen refresh token'ı doğrular, eğer geçerliyse yeni bir access token ve yeni bir refresh token oluşturur ve döndürür.
    public async Task<LoginResultDto> RefreshTokenAsync(RefreshTokenRequestDto dto, string deviceInfo, CancellationToken cancellationToken)
    {
        ClaimsPrincipal? principal; //refresh token'ı doğrulamak için access token içindeki claimleri okumamız gerekiyor. ancak access token'ın süresi dolmuş olabilir. bu yüzden access token'ı doğrularken süresinin dolmuş olmasını göz ardı ediyoruz. GetPrincipalFromExpiredToken metodu, access token'ı doğrular ve içindeki claimleri okur. eğer token geçersizse veya doğrulanamazsa bir hata fırlatır.
        try
        {
            principal = GetPrincipalFromExpiredToken(dto.AccessToken); //kısa açıklama : refresh token'ı doğrulamak için access token içindeki claimleri okumamız gerekiyor. ancak access token'ın süresi dolmuş olabilir. bu yüzden access token'ı doğrularken süresinin dolmuş olmasını göz ardı ediyoruz. GetPrincipalFromExpiredToken metodu, access token'ı doğrular ve içindeki claimleri okur. eğer token geçersizse veya doğrulanamazsa bir hata fırlatır.
        }
        catch
        {
            return Fail("Geçersiz Token"); //eğer access token geçersizse veya doğrulanamazsa, refresh token'ı doğrulama sürecine devam edemeyiz çünkü refresh token'ı doğrulamak için access token içindeki claimlere ihtiyacımız var. bu yüzden bu durumda başarısız bir login sonucu döndürürüz ve hata mesajını belirtiriz.
        }

        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier); //access token içindeki claimlerden kullanıcının id'sini alırız. bu id'yi kullanarak refresh token'ı doğrulama sürecine devam ederiz. eğer userId null ise, bu durumda access token'dan kullanıcı id'sini alamamışız demektir ve bu durumda refresh token'ı doğrulama sürecine devam edemeyiz. bu yüzden bu durumda başarısız bir login sonucu döndürürüz ve hata mesajını belirtiriz.
        if (userId == null) //eğer access token içindeki claimlerden kullanıcı id'sini alamamışsak, refresh token'ı doğrulama sürecine devam edemeyiz. bu yüzden bu durumda başarısız bir login sonucu döndürürüz ve hata mesajını belirtiriz.
        {
            return Fail("Geçersiz Token");
        }
        var storedToken = await _refreshTokenDal.GetValidTokenAsync(dto.RefreshToken, Guid.Parse(userId), cancellationToken); //refresh token'ı doğrulamak için veritabanında bu token'ın geçerli olup olmadığını kontrol ederiz. GetValidTokenAsync metodu, verilen refresh token'ı ve kullanıcı id'sini kullanarak veritabanında bu token'ın geçerli olup olmadığını kontrol eder. eğer token geçerliyse, token bilgilerini döndürür. eğer token geçersizse veya bulunamazsa null döndürür. bu yüzden eğer storedToken null ise, bu durumda refresh token geçersiz veya süresi dolmuş demektir ve bu durumda başarısız bir login sonucu döndürürüz ve hata mesajını belirtiriz.
        if (storedToken == null)
        {
            return Fail("Refresh Token Geçersiz veya süresi dolmuş");
        }

        storedToken.IsRevoked = true; //refresh token'ı tek kullanımlık yapıyoruz. yani bu token'ı kullanarak yeni bir access token aldığımızda, bu refresh token'ı geçersiz hale getiriyoruz. böylece aynı refresh token'ı tekrar kullanarak yeni bir access token alınamaz. bu, güvenlik açısından önemlidir çünkü eğer bir refresh token çalınırsa, saldırgan bu token'ı kullanarak sürekli yeni access token'lar alabilir. tek kullanımlık refresh token'lar, bu riski azaltır.
        await _refreshTokenDal.SaveChangesAsync(cancellationToken); //refresh token'ı geçersiz hale getirdikten sonra, bu değişikliği veritabanına kaydetmemiz gerekiyor. SaveChangesAsync metodu, yapılan değişiklikleri veritabanına kaydeder.

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Fail("Kullanıcı Bulunamadı");
        } //refresh token doğrulama süreci başarılı olduysa, şimdi yeni bir access token ve yeni bir refresh token oluşturabiliriz. yeni access token'ı CreateAccessTokenAsync metodu ile oluştururuz. bu metot, verilen kullanıcı bilgilerine göre yeni bir access token oluşturur ve döndürür. yeni refresh token'ı ise CreateRefreshTokenAsync metodu ile oluştururuz. bu metot, verilen kullanıcı bilgilerine ve cihaz bilgilerine göre yeni bir refresh token oluşturur, veritabanına kaydeder ve token string'ini döndürür.

        var newAccessToken = await CreateAccessTokenAsync(user); //yeni access token oluşturulur. bu metot, verilen kullanıcı bilgilerine göre yeni bir access token oluşturur ve döndürür.

        var newRefreshToken = await CreateRefreshTokenAsync(user, deviceInfo, cancellationToken); //yeni refresh token oluşturulur. bu metot, verilen kullanıcı bilgilerine ve cihaz bilgilerine göre yeni bir refresh token oluşturur, veritabanına kaydeder ve token string'ini döndürür.

        return new LoginResultDto //client tarafında bu yeni access token'ı kullanarak uygulamaya erişim sağlanabilir ve yeni refresh token'ı saklayarak ileride access token'ın süresi dolduğunda bu refresh token'ı kullanarak yeni bir access token alınabilir.
        {
            Success = true,
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };


    }

    //bu metot, bir kullanıcının tüm refresh token'larını geçersiz hale getirmek için kullanılır. bu genellikle kullanıcı şifresini değiştirdiğinde veya hesabında şüpheli bir etkinlik tespit edildiğinde yapılır. bu metot, verilen kullanıcı id'sine sahip kullanıcıyı bulur, eğer kullanıcı bulunursa, kullanıcının security stamp'ini günceller ve ardından veritabanında bu kullanıcıya ait tüm refresh token'ları geçersiz hale getirir. security stamp'in güncellenmesi, kullanıcının mevcut access token'larının da geçersiz hale gelmesini sağlar çünkü access token'lar oluşturulurken kullanıcının security stamp'i de token içine dahil edilir. security stamp değiştiğinde, mevcut access token'lar doğrulanamaz hale gelir.
    public async Task RevokeTokensAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.UpdateSecurityStampAsync(user); //securitystamp kısaca kullanıcının güvenlik durumunu temsil eder. bu değer değiştiğinde, kullanıcının mevcut access token'ları geçersiz hale gelir. bu yüzden kullanıcıya ait refresh token'ları geçersiz hale getirmeden önce security stamp'i güncelliyoruz. böylece kullanıcıya ait tüm access token'lar ve refresh token'lar geçersiz hale gelmiş olur.

            await _refreshTokenDal.RevokeAllByUserAsync(user.Id, cancellationToken); // böylece bu kullanıcıya ait hiçbir refresh token geçerli olmaz ve bu kullanıcı yeni access token'lar almak için yeni refresh token'lar oluşturmak zorunda kalır.
        }
    }

    //bu metot, verilen kullanıcı bilgilerine ve cihaz bilgilerine göre yeni bir refresh token oluşturur, veritabanına kaydeder ve token string'ini döndürür. refresh token, access token'ın süresi dolduğunda yeni bir access token almak için kullanılır. refresh token daha uzun süre geçerlidir ve genellikle client tarafında saklanır. client, access token'ın süresi dolduğunda refresh token'ı kullanarak yeni bir access token talep eder. bu metot, yeni bir refresh token oluşturur ve veritabanına kaydeder.
    public async Task<string> CreateRefreshTokenAsync(AppUser user, string? deviceInfo, CancellationToken cancellation = default)
    {
        var tokenBytes = new byte[64]; //refresh token'ı oluşturmak için rastgele byte dizisi oluşturulur. bu byte dizisi daha sonra base64 string'e dönüştürülerek refresh token string'i elde edilir. 64 byte'lık bir dizi, yeterince uzun ve güvenli bir refresh token oluşturmak için genellikle yeterlidir.
        using var rng = RandomNumberGenerator.Create(); //RandomNumberGenerator, kriptografik olarak güvenli rastgele sayılar üretmek için kullanılan bir sınıftır. bu sınıf, refresh token'ı oluşturmak için rastgele byte dizisi oluşturmakta kullanılır. Create metodu, RandomNumberGenerator sınıfının bir örneğini oluşturur. GetBytes metodu ise belirtilen byte dizisini rastgele verilerle doldurur.
        rng.GetBytes(tokenBytes); //tokenBytes dizisi rastgele verilerle doldurulur. bu diziyi base64 string'e dönüştürerek refresh token

        var tokenString = Convert.ToBase64String(tokenBytes); //tokenBytes dizisi base64 string'e dönüştürülür. bu string, refresh token olarak kullanılacaktır. base64 string'ler, ikili verileri metin formatında temsil etmek için kullanılır ve genellikle token'lar gibi verilerin güvenli bir şekilde iletilmesi için tercih edilir.

        var refreshToken = new RefreshToken  //RefreshToken nesnesi oluşturulur ve içine token string'i, expiration tarihi, cihaz bilgisi ve kullanıcı id'si gibi bilgiler atanır. bu nesne, veritabanına kaydedilecek olan refresh token'ı temsil eder.
        {
            Token = tokenString, //oluşturulan token string'i refresh token nesnesine atanır.
            ExpiresAt = DateTime.UtcNow.AddDays(7), // refresh token'ın geçerlilik süresi belirlenir. burada 7 gün olarak ayarlanmıştır, ancak bu süre ihtiyaca göre değiştirilebilir. refresh token'lar genellikle access token'lardan daha uzun süre geçerli olacak şekilde tasarlanır çünkü refresh token'lar access token'ların süresi dolduğunda yeni access token'lar almak için kullanılır.
            DeviceInfo = deviceInfo, //refresh token'ı hangi cihazda kullanıldığını belirtmek için cihaz bilgisi atanır. bu bilgi, refresh token'ların yönetimi ve güvenliği açısından faydalı olabilir. örneğin, bir kullanıcının sadece belirli cihazlarda refresh token'larını geçerli kılmak veya şüpheli cihazlardan gelen refresh token taleplerini engellemek için bu bilgiyi kullanabiliriz.
            UserId = user.Id //refresh token'ın hangi kullanıcıya ait olduğunu belirtmek için kullanıcı id'si atanır. bu bilgi, refresh token'ların doğrulanması ve yönetimi açısından önemlidir. refresh token doğrulama sürecinde, gelen refresh token'ın hangi kullanıcıya ait olduğunu bilmemiz gerekir ve bu bilgi sayesinde veritabanında bu kullanıcıya ait refresh token'ları kontrol edebiliriz.
        };

        await _refreshTokenDal.AddAsync(refreshToken, cancellation);
        return tokenString;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    //bu metot, süresi dolmuş bir access token'ı doğrulamak ve içindeki claimleri okumak için kullanılır. token doğrulanırken süresinin dolmuş olmasını göz ardı ederiz çünkü refresh token'ı kullanarak yeni bir access token alırken access token'ın süresinin dolmuş olması normaldir. bu metot, token'ı doğrular ve içindeki claimleri okur. eğer token geçersizse veya doğrulanamazsa bir hata fırlatır.
    {
        var key = new SymmetricSecurityKey //token doğrulamak için kullanılan gizli anahtar. bu anahtar token'ı oluştururken kullanılan anahtarla aynı olmalıdır. token doğrulanırken bu anahtar kullanılarak token'ın geçerliliği kontrol edilir.
           (
               Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)
           );

        var validation = new TokenValidationParameters //token doğrulama parametreleri. bu parametreler token doğrulama sürecinde kullanılır. ValidateIssuer ve ValidateAudience, token'ın issuer ve audience değerlerinin doğrulanıp doğrulanmayacağını belirtir. ValidIssuer ve ValidAudience, token'ın geçerli olması için beklenen issuer ve audience değerlerini belirtir. IssuerSigningKey, token'ı doğrulamak için kullanılan anahtarı belirtir. ValidateLifetime, token'ın süresinin dolmuş olmasını göz ardı edip etmeyeceğimizi belirtir.
        {
            ValidateIssuer = true, // token'ın issuer'ını doğrula
            ValidateAudience = true, // token'ın audience'ını doğrula
            ValidIssuer = _configuration["Jwt:Issuer"], // token'ın geçerli olması için beklenen issuer değeri
            ValidAudience = _configuration["Jwt:Audience"], // token'ın geçerli olması için beklenen audience değeri
            IssuerSigningKey = key, // token'ı doğrulamak için kullanılan anahtar
            ValidateLifetime = false // token'ın süresinin dolmuş olmasını göz ardı et. refresh token'ı kullanarak yeni bir access token alırken access token'ın süresinin dolmuş olması normaldir, bu yüzden bu değeri false yapıyoruz.
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _); // buradaki _ discard pattern kullanımıdır. ValidateToken metodu, token'ı doğrular ve içindeki claimleri okur. eğer token geçersizse veya doğrulanamazsa bir hata fırlatır. eğer token geçerliyse, token içindeki claimleri içeren bir ClaimsPrincipal nesnesi döndürür. bu nesne, refresh token doğrulama sürecinde kullanılarak token içindeki claimlere erişmemizi sağlar.
    }

    private static LoginResultDto Fail(string error) //bu metot, refresh token doğrulama sürecinde herhangi bir adımda hata oluşursa, bu hatayı belirtmek için kullanılır. bu metot, başarısız bir login sonucu döndürür ve içinde hata mesajını içerir. böylece client tarafında bu hatayı göstermek veya işlemek mümkün olur.
    {
        return new LoginResultDto
        {
            Success = false,
            Error = error

        };
    }

     
}
