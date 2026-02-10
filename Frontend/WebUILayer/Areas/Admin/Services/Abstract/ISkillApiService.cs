using DtoLayer.SkillDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface ISkillApiService:IGenericApiService<SkillDto,CreateSkillDto,UpdateSkillDto>
    {
    }
}
