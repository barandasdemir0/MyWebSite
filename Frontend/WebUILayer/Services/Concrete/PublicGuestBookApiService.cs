using DtoLayer.GuestBookDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicGuestBookApiService : PublicReadApiService<GuestBookListDto>, IPublicGuestBookApiService
{
    public PublicGuestBookApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "guestbooks")
    {
    }
}
