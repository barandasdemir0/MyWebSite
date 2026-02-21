using Microsoft.AspNetCore.Identity;

namespace EntityLayer.Entities;

public sealed class AppUser:IdentityUser<Guid>
{
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //hangi 2fa yöntemini tercih ettiğin  email or google authenticator

    public TwoFactorProvider Preferred2FAProvider { get; set; } = TwoFactorProvider.None;
}
