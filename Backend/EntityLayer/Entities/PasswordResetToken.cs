using SharedKernel.Shared;

namespace CV.EntityLayer.Entities;

public class PasswordResetToken
{
    public PasswordResetToken()
    {
        Id = Guid.CreateVersion7();
        Token = SecurityHelper.GenerateSecureToken();
    }
    public Guid Id { get; protected set; }
    public string Token { get; protected set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } // geçerlilik süresi
    public bool IsUsed { get; set; } //jeton daha önce kullanıldımı?
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; //jeton ne zaman oluşturuldu 
}
