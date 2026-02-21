namespace DtoLayer.AuthDtos;

public class Setup2FAResultDto
{
    public string? QrCodeImageBase64 { get; set; }
    public string? ManuelEntryKey { get; set; }
}
