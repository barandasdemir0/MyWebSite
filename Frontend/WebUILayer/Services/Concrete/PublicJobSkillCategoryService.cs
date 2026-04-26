using DtoLayer.JobSkillCategoryDtos;
using WebUILayer.Areas.Admin.Services.Abstract;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicJobSkillCategoryService : PublicReadApiService<JobSkillCategoryDto>,IPublicJobSkillCategoryService
{
    public PublicJobSkillCategoryService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "jobskillcategories")
    {
    }
}
