namespace EntityLayer.Entities;

public class PasswordResetToken
{
    public PasswordResetToken()
    {
        Id = Guid.CreateVersion7();
        Token = Guid.CreateVersion7().ToString("N");//32 char hex
        
    }
    public Guid Id { get; protected set; }
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
