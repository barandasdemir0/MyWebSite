namespace DtoLayer.AuthDtos;

public class LoginResultDto
{
    public bool Success { get; set; }
    public bool RequiresTwoFactor { get; set; }
    public string? UserId { get; set; }
    public string? Token { get; set; }
    public string? Error { get; set; }
}
