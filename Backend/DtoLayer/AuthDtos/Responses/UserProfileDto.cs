using SharedKernel.Enums;

namespace DtoLayer.AuthDtos.Responses;

public record UserProfileDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public bool TwoFactorEnabled { get; init; }
    public TwoFactorProvider Preferred2FAProvider { get; init; } = TwoFactorProvider.None;
}
