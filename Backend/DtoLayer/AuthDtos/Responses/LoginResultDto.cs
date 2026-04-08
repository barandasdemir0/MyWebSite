namespace DtoLayer.AuthDtos.Responses;

public record LoginResultDto
{
    public bool Success { get; init; }
    public bool RequiresTwoFactor { get; init; }
    public string? UserId { get; init; }
    public string? Token { get; init; }
    public string? Error { get; init; }
    public string? RefreshToken { get; init; }  // EKLE

}
