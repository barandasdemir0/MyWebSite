using DtoLayer.AboutDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicAboutApiService : PublicReadApiService<AboutDto>, IPublicAboutApiService
{
    public PublicAboutApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "abouts")
    {
    }
}
