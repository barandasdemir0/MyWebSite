namespace DtoLayer.GuestBookDtos;

public class OAuthUserProfileDto
{
    public string AuthProvider { get; set; } = string.Empty;
    public string AuthProviderId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorAvatarUrl { get; set; } = string.Empty;
    public string? AuthorProfileUrl { get; set; }
}
