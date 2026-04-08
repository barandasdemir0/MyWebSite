namespace DtoLayer.AuthDtos.Responses;

public record Setup2FAResultDto
{
    public string? QrCodeImageBase64 { get; init; }
    public string? ManualEntryKey { get; init; }
}
