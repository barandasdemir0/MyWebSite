using DtoLayer.SkillDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class SkillApiService : GenericApiService<SkillDto, CreateSkillDto, UpdateSkillDto>, ISkillApiService
    {
        public SkillApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "skills")
        {
        }
    }
}
