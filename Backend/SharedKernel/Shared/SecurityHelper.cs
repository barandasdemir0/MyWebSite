using System.Security.Cryptography;

namespace SharedKernel.Shared;

public static class SecurityHelper
{
    public static string GenerateSecureToken(int length = 32)
    {
        var bytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToHexString(bytes);
    }
}
