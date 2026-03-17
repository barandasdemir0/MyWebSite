using EntityLayer.Entities;

namespace DtoLayer.AuthDtos;

public class VerifyResetOtpDto
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public TwoFactorProvider provider { get; set; } = TwoFactorProvider.Email;
}
