using Microsoft.AspNetCore.Identity;
using SharedKernel.Enums;

namespace CV.EntityLayer.Entities;

public sealed class AppUser:IdentityUser<Guid>
{
    public string? Name { get; set; }
    public string? Surname { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //hangi 2fa yöntemini tercih ettiğin  email or google authenticator

    public TwoFactorProvider Preferred2FAProvider { get; set; } = TwoFactorProvider.None;
    public bool IsApproved { get; set; } = false;  // Admin onayı Kullanıcı kayıt oldu ama hemen sisteme giremez



    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    //ıcollection tuttuk çünkü birden fazla refreshtoken olabilir peki neden list değilde koleksiyon tuttum bunun sebebi burası bir list olabilir yarın hashset olabilir bu yüzden buraya bir grup veri dedim  bu durumda ıcollectionum collection olurken benim verdiğim listtir hashsette olabilirdi

}
