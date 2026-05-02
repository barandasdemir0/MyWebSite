using DtoLayer.ExperienceDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicExperienceApiService : PublicReadApiService<ExperienceDto>, IPublicExperienceApiService
{
    public PublicExperienceApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "experiences")
    {
    }
}
