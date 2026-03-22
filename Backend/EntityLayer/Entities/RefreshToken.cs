namespace CV.EntityLayer.Entities;

public class RefreshToken
{
    public RefreshToken()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; protected set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } //süresi dolma
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? DeviceInfo { get; set; }
    public bool IsRevoked { get; set; } //iptal edildi

    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

}
