using DtoLayer.GuestBookDtos;

namespace WebUILayer.Services.Abstract;

public interface IPublicGuestBookApiService:IPublicReadApiService<GuestBookDto>
{
    //Task<List<GuestBookDto>> GetLatestAsync(int count);
}
