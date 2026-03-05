using EntityLayer.Entities;

namespace DtoLayer.AuthDtos;

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public TwoFactorProvider Preferred2FAProvider { get; set; } = TwoFactorProvider.None;
}
