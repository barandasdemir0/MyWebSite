using System.Security.Claims;

namespace WebUILayer.Extension;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

    public static string GetUserEmail(this ClaimsPrincipal email)
    {
        return email?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    }

}
