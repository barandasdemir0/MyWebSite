using EntityLayer.Entities;

namespace DtoLayer.AuthDtos;

public class Toggle2FADto
{
    public bool Enable { get; set; }
    public TwoFactorProvider Provider { get; set; } = TwoFactorProvider.Email;
}
