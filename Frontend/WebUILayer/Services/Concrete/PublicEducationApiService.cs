using DtoLayer.EducationDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicEducationApiService : PublicReadApiService<EducationDto>, IPublicEducationApiService
{
    public PublicEducationApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "educations")
    {
    }
}
