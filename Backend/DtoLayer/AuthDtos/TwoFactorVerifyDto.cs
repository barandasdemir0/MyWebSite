using EntityLayer.Entities;

namespace DtoLayer.AuthDtos;

public class TwoFactorVerifyDto
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public TwoFactorProvider Provider { get; set; } = TwoFactorProvider.Email;
}
