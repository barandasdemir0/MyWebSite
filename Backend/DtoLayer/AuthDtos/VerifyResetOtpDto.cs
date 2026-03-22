using SharedKernel.Enums;

namespace DtoLayer.AuthDtos;

public class VerifyResetOtpDto
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public TwoFactorProvider Provider { get; set; } = TwoFactorProvider.Email;
}
