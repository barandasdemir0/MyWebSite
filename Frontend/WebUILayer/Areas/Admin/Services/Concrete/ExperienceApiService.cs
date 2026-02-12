using DtoLayer.ExperienceDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class ExperienceApiService : GenericApiService<ExperienceDto, CreateExperienceDto, UpdateExperienceDto>, IExperienceApiService
    {
        public ExperienceApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "experiences")
        {
        }

        public async Task<List<ExperienceDto>> GetAllAdminAsync()
        {
            var query = await _httpClient.GetFromJsonAsync<List<ExperienceDto>>($"{_endpoint}/admin-all");
            if (query == null)
            {
                return new List<ExperienceDto>();
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
}
