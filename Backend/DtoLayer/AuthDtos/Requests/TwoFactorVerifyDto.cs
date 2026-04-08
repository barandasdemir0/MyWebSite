using SharedKernel.Enums;

namespace DtoLayer.AuthDtos.Requests;

public class TwoFactorVerifyDto
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public TwoFactorProvider Provider { get; set; } = TwoFactorProvider.Email;
    public string? DeviceInfo { get; set; }

}
