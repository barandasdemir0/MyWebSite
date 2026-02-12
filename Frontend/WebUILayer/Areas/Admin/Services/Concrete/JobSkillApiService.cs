using DtoLayer.JobSkillsDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class JobSkillApiService : GenericApiService<JobSkillDto, CreateJobSkillDto, UpdateJobSkillDto>, IJobSkillApiService
{
    public JobSkillApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "jobskills")
    {
    }

    public async Task<List<JobSkillDto>> GetAllAdminAsync()
    {
        var query = await _httpClient.GetFromJsonAsync<List<JobSkillDto>>($"{_endpoint}/admin-all");
        if (query == null)
        {
            return new List<JobSkillDto>();
        }
        return query;
    }

    public async Task RestoreAsync(Guid guid)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/restore/{guid}", null);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
