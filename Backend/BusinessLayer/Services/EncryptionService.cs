using Microsoft.AspNetCore.DataProtection;

namespace BusinessLayer.Services;

public class EncryptionService
{
    private readonly IDataProtector _protector;
    public EncryptionService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("ChatbotApiKey");
    }

    public string Encrypt(string text) => _protector.Protect(text);

    public string Decrypt(string text) => _protector.Unprotect(text);
}
