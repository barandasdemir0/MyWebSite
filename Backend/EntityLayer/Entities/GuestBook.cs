namespace CV.EntityLayer.Entities;


public sealed class GuestBook:BaseEntity
{
    // OAuth Bilgileri
    public string AuthProvider { get; set; } = string.Empty;
    public string AuthProviderId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorAvatarUrl { get; set; } = string.Empty;
    public string? AuthorProfileUrl { get; set; }

    // Ortak Alan
    public string Message { get; set; } = string.Empty;

    // Moderasyon
    public bool IsApproved { get; set; } = false;                // Moderasyon sonrası
}
