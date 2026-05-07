using DtoLayer.AboutDtos;

namespace WebUILayer.Services.Abstract;

public interface IPublicAboutApiService:IPublicReadApiService<AboutDto>
{
    Task<AboutDto> GetPublicAboutsAsync();
}
