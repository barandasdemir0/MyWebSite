using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
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
        } //kullanıcı doğrulama kısmında bulunmazsa false döndürürüz, bu durumda kullanıcıya "Kullanıcı Bulunamadı" mesajı gösterilir.
        var valid = await _userManager.VerifyTwoFactorTokenAsync( 
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, Code
            );  //: Identity'ye "Bu kodu E-posta kodu gibi değil, TOTP (Time-based One-Time Password) algoritmasına göre kontrol et" diyoruz.// kullanıcının doğrulama kodunu doğrularız, eğer kod geçersizse false döndürürüz, bu durumda kullanıcıya "Geçersiz veya süresi dolmuş kod" mesajı gösterilir.
        if (!valid) //: Eğer kod geçersizse false döndürürüz, bu durumda kullanıcıya "Geçersiz veya süresi dolmuş kod" mesajı gösterilir.
        {
            return false;
        }
        await _userManager.SetTwoFactorEnabledAsync(user, true); //: Kullanıcının iki faktörlü kimlik doğrulamayı etkinleştiririz.
        user.Preferred2FAProvider = TwoFactorProvider.Authenticator; //: Kullanıcının tercih ettiği iki faktörlü kimlik doğrulama sağlayıcısını "Authenticator" olarak ayarlarız.
        await _userManager.UpdateAsync(user); //: Kullanıcı bilgilerini güncelleriz.
        return true;//: Başarılı bir şekilde doğrulama kodu geçerliyse ve iki faktörlü kimlik doğrulama etkinleştirilmişse true döndürürüz, bu durumda kullanıcıya "Authenticator uygulamanız başarıyla kuruldu" mesajı gösterilir.
    }

    public async Task<bool> SendMailOtpAsync(string userId, CancellationToken cancellationToken) //: Kullanıcıya e-posta ile tek kullanımlık doğrulama kodu gönderir.
    {
        var user = await _userManager.FindByIdAsync(userId); //: Kullanıcıyı veritabanında buluruz, eğer kullanıcı bulunmazsa false döndürürüz, bu durumda kullanıcıya "Kullanıcı Bulunamadı" mesajı gösterilir.
        if (user?.Email == null) 
        {
            return false;
        }

        await _userManager.UpdateSecurityStampAsync(user); //: Kullanıcının güvenlik damgasını güncelleriz, böylece önceki doğrulama kodları geçersiz hale gelir ve yeni bir kod oluşturulmasını sağlar.
        var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider); //: Kullanıcı için yeni bir doğrulama kodu oluştururuz, bu kod e-posta sağlayıcısına göre oluşturulur.

        await _emailService.SendAsync(user.Email, "Giriş Doğrulama Kodunuz", $"Giriş kodunuz: <b>{code}</b><br/>Bu kod 5 dakika geçerlidir.", cancellationToken); //: Kullanıcının e-posta adresine doğrulama kodunu göndeririz, eğer e-posta gönderme işlemi başarısız olursa false döndürürüz, bu durumda kullanıcıya "E-posta gönderilemedi" mesajı gösterilir.
        return true;
    }

    public async Task<Setup2FAResultDto> SetupAuthenticatorAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId); //: Kullanıcıyı veritabanında buluruz, eğer kullanıcı bulunmazsa NotFoundException fırlatırız, bu durumda kullanıcıya "Kullanıcı Bulunamadı" mesajı gösterilir.
        if (user == null)
        {
            throw new NotFoundException("Kullanıcı bulunamadı",userId);
        }
        await _userManager.ResetAuthenticatorKeyAsync(user); //: Kullanıcının mevcut doğrulama anahtarını sıfırlar, böylece yeni bir anahtar oluşturulmasını sağlar. Bu adım, kullanıcının daha önce oluşturduğu QR kodunun geçersiz hale gelmesini sağlar ve yeni bir QR kodu oluşturulmasına olanak tanır.
        var key = await _userManager.GetAuthenticatorKeyAsync(user); //: Kullanıcının yeni doğrulama anahtarını alırız, bu anahtar TOTP algoritmasına göre oluşturulur ve QR kodu oluşturmak için kullanılır.


        //: QR kodu oluşturmak için otpauth URI'si oluştururuz, bu URI TOTP algoritmasına göre oluşturulur ve QR kodu oluşturmak için kullanılır. URI formatı şu şekildedir: otpauth://totp/{issuer}:{account}?secret={secret}&issuer={issuer}
        var otpUri = $"otpauth://totp/{Uri.EscapeDataString("barandasdemir.com")}:" + 
            $"{Uri.EscapeDataString(user.Email!)}?secret={key}&issuer=" +
            Uri.EscapeDataString("barandasdemir.com");  //  Uri.EscapeDataString("barandasdemir.com"); bu nedir? : Bu, URI bileşenlerini güvenli hale getirmek için kullanılan bir yöntemdir. Özellikle, URI içinde özel karakterler veya boşluklar varsa, bu karakterlerin uygun şekilde kodlanmasını sağlar. Örneğin, "barandasdemir.com" ifadesi URI içinde doğrudan kullanıldığında sorunlara yol açabilir, ancak Uri.EscapeDataString("barandasdemir.com") ifadesi bu tür sorunları önler ve URI'nin doğru şekilde oluşturulmasını sağlar.


        using var qrGenerator = new QRCodeGenerator(); //: QRCodeGenerator sınıfını kullanarak QR kodu oluşturmak için bir örnek oluştururuz. Bu sınıf, QRCoder kütüphanesi tarafından sağlanır ve QR kodu oluşturmak için gerekli yöntemleri içerir.
        var qrData = qrGenerator.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q); //: QR kodu oluşturmak için CreateQrCode yöntemini kullanırız, bu yöntem otpauth URI'sini alır ve QR kodu oluşturur. ECCLevel.Q parametresi, QR kodunun hata düzeltme seviyesini belirler, bu durumda Q seviyesi seçilmiştir, bu da orta düzeyde hata düzeltme sağlar ve QR kodunun daha güvenilir olmasını sağlar. 
        
        using var qrCode = new PngByteQRCode(qrData); //: Oluşturulan QR kodu verilerini kullanarak bir PngByteQRCode nesnesi oluştururuz, bu nesne QR kodunu PNG formatında byte dizisi olarak oluşturmak için kullanılır. Bu adım, QR kodunun görsel olarak temsil edilmesini sağlar ve daha sonra bu byte dizisi base64 formatına dönüştürülerek istemciye gönderilir.
        var qrBytes = qrCode.GetGraphic(5); // : QR kodunu PNG formatında byte dizisi olarak oluştururuz, bu byte dizisi QR kodunun görsel temsilini içerir. GetGraphic yöntemi, QR kodunun boyutunu ve diğer görsel özelliklerini belirlemek için kullanılır, bu durumda 5 değeri seçilmiştir, bu da QR kodunun her bir modülünün (küçük karelerin) 5 piksel genişliğinde olmasını sağlar. Bu adım, QR kodunun görsel kalitesini ve okunabilirliğini etkiler.
        var base64 = Convert.ToBase64String(qrBytes); //: QR kodu byte dizisini base64 formatına dönüştürürüz, bu format QR kodunun görsel temsilini metin olarak ifade eder ve istemciye gönderilmek üzere uygun hale getirir. Base64 formatı, ikili verilerin metin tabanlı bir şekilde temsil edilmesini sağlar ve genellikle web uygulamalarında görsel verilerin iletilmesi için kullanılır. Bu adım, QR kodunun istemci tarafından kolayca görüntülenebilmesini sağlar.

        return new Setup2FAResultDto //: Oluşturulan QR kodu görselini ve manuel giriş anahtarını içeren bir Setup2FAResultDto nesnesi döndürürüz, bu nesne istemciye gönderilir ve kullanıcıya QR kodunu tarayarak veya manuel giriş anahtarını kullanarak iki faktörlü kimlik doğrulamayı kurma imkanı sağlar. QrCodeImageBase64 özelliği, QR kodunun base64 formatında görsel temsilini içerirken, ManualEntryKey özelliği ise kullanıcıya manuel olarak girmesi için sağlanan doğrulama anahtarını içerir.
        {
            QrCodeImageBase64 = $"data:image/png;base64,{base64}", 
            ManualEntryKey = key
        };
    }

    public async Task<bool> Toggle2FAAsync(string userId, Toggle2FADto toggle2FADto, CancellationToken cancellationToken)
    { //: Kullanıcının iki faktörlü kimlik doğrulamayı etkinleştirme veya devre dışı bırakma işlemini gerçekleştirir. Bu yöntem, kullanıcının tercih ettiği iki faktörlü kimlik doğrulama sağlayıcısını günceller ve iki faktörlü kimlik doğrulamayı etkinleştirir veya devre dışı bırakır. Eğer kullanıcı bulunmazsa false döndürürüz, bu durumda kullanıcıya "Kullanıcı Bulunamadı" mesajı gösterilir.
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
            //: Kullanıcı bulunmazsa false döndürürüz, bu durumda kullanıcıya "Kullanıcı Bulunamadı" mesajı gösterilir.
        }
        await _userManager.SetTwoFactorEnabledAsync(user, toggle2FADto.Enable); //: Kullanıcının iki faktörlü kimlik doğrulamayı etkinleştirir veya devre dışı bırakır, toggle2FADto.Enable değeri true ise iki faktörlü kimlik doğrulama etkinleştirilir, false ise devre dışı bırakılır.
        user.Preferred2FAProvider = toggle2FADto.Enable ? toggle2FADto.Provider : TwoFactorProvider.None; //: Kullanıcının tercih ettiği iki faktörlü kimlik doğrulama sağlayıcısını günceller, toggle2FADto.Enable değeri true ise toggle2FADto.Provider değeri kullanılır, false ise TwoFactorProvider.None olarak ayarlanır, bu da kullanıcının artık herhangi bir iki faktörlü kimlik doğrulama sağlayıcısı tercih etmediği anlamına gelir.
        await _userManager.UpdateAsync(user); //: Kullanıcı bilgilerini güncelleriz, böylece yapılan değişiklikler veritabanına kaydedilir. Bu adım, kullanıcının iki faktörlü kimlik doğrulama tercihlerini ve durumunu güncel tutmak için önemlidir.
        return true;
    }


    //: Kullanıcının iki faktörlü kimlik doğrulama kodunu doğrular ve geçerliyse erişim token'ı ve yenileme token'ı oluşturur. Bu yöntem, kullanıcının girdiği doğrulama kodunu kontrol eder ve eğer kod geçerliyse kullanıcıya erişim token'ı ve yenileme token'ı sağlar. Eğer kullanıcı bulunmazsa veya doğrulama kodu geçersizse uygun hata mesajları içeren bir LoginResultDto nesnesi döndürür.
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
        } //: Kullanıcı bulunmazsa uygun hata mesajları içeren bir LoginResultDto nesnesi döndürürüz, bu durumda kullanıcıya "Kullanıcı Bulunamadı" mesajı gösterilir.

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

        return new LoginResultDto//: Doğrulama kodu geçerliyse kullanıcıya erişim token'ı ve yenileme token'ı sağlayan bir LoginResultDto nesnesi döndürürüz, bu durumda kullanıcıya "Giriş başarılı" mesajı gösterilir.
        {
            Success = true, //: Doğrulama kodu geçerliyse true olarak ayarlanır, bu da giriş işleminin başarılı olduğunu gösterir.
            Token = await _tokenService.CreateAccessTokenAsync(user),//: Kullanıcı için yeni bir erişim token'ı oluştururuz, bu token kullanıcının kimliğini doğrulamak ve yetkilendirmek için kullanılır. Erişim token'ı genellikle kısa ömürlüdür ve kullanıcıya belirli bir süre boyunca erişim sağlar.
            RefreshToken = await _tokenService.CreateRefreshTokenAsync(user, dto.DeviceInfo) //: Kullanıcı için yeni bir yenileme token'ı oluştururuz, bu token kullanıcının erişim token'ını yenilemek için kullanılır. Yenileme token'ı genellikle daha uzun ömürlüdür ve kullanıcıya sürekli erişim sağlar, böylece kullanıcı sık sık yeniden giriş yapmak zorunda kalmaz.

        };
    }

   

    

  
}
