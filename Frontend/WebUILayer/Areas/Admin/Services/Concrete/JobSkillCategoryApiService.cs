using DtoLayer.JobSkillCategoryDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class JobSkillCategoryApiService : GenericApiService<JobSkillCategoryDto, CreateJobSkillCategoryDto, UpdateJobSkillCategoryDto>, IJobSkillCategoryService
{
    public JobSkillCategoryApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "jobskillcategories")  // endpoint = "jobskillcategories"
    {
    }

    public async Task<List<JobSkillCategoryDto>> GetAdminAllAsync()
    {
        var query = await _httpClient.GetFromJsonAsync<List<JobSkillCategoryDto>>($"{_endpoint}/admin-all");
        if (query == null)
        {
            return new List<JobSkillCategoryDto>();
        }
        return query;
    }

    public async Task RestoreAsync(Guid guid)
    {
        var query = await _httpClient.PutAsync($"{_endpoint}/restore/{guid}", null);
        if (!query.IsSuccessStatusCode)
        {
            var error = await query.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
