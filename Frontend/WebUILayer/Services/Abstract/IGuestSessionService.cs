using DtoLayer.GuestBookDtos;

namespace WebUILayer.Services.Abstract;

public interface IGuestSessionService
{
    CreateGuestBookDto? GetCurrentGuest();
    void SetCurrentGuest(CreateGuestBookDto guest);
    void clearGuest();


}
