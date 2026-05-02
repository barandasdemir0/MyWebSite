using DtoLayer.SocialMediaDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicSocialMediaApiService : PublicReadApiService<SocialMediaDto>, IPublicSocialMediaApiService
{
    public PublicSocialMediaApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "socialmedias")
    {
    }
}
