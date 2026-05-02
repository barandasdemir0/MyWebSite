using DtoLayer.GuestBookDtos;
using System.Text.Json;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class GuestSessionService : IGuestSessionService
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    public GuestSessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void clearGuest()
    {
        _httpContextAccessor.HttpContext?.Session.Remove("GuestUser");
    }

    public CreateGuestBookDto? GetCurrentGuest()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return null;
        var json = session.GetString("GuestUser");
        return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<CreateGuestBookDto>(json);
    }

    public void SetCurrentGuest(CreateGuestBookDto guest)
    {
        var json = JsonSerializer.Serialize(guest);
        _httpContextAccessor.HttpContext?.Session.SetString("GuestUser", json);
    }
}
