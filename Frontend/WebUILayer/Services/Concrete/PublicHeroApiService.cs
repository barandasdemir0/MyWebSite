using DtoLayer.HeroDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicHeroApiService : PublicReadApiService<HeroDto>, IPublicHeroApiService
{
    public PublicHeroApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "heroes")
    {
    }
}
