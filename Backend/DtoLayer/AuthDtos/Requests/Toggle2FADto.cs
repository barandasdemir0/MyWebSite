using SharedKernel.Enums;

namespace DtoLayer.AuthDtos.Requests;

public class Toggle2FADto
{
    public bool Enable { get; set; }
    public TwoFactorProvider Provider { get; set; } = TwoFactorProvider.Email;
}
