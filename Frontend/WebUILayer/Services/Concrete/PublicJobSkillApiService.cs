using DtoLayer.JobSkillsDtos;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicJobSkillApiService : PublicReadApiService<JobSkillDto>, IPublicJobSkillApiService
{
    public PublicJobSkillApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "jobskills")
    {
    }
}
