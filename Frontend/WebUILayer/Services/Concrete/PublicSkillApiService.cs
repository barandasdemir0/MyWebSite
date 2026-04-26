using DtoLayer.SkillDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicSkillApiService : PublicReadApiService<SkillDto>, IPublicSkillApiService
{
    public PublicSkillApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "skills")
    {
    }
}
