namespace WebUILayer.Services.Abstract;

public interface ICookieAuthService
{
    Task SignInWithJwtAsync(string accessToken, string? refreshToken = null, bool rememberMe = false);
    Task SignOutAsync();
}
