using DtoLayer.EducationDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class EducationApiService : GenericApiService<EducationDto, CreateEducationDto, UpdateEducationDto>, IEducationApiService
    {
        public EducationApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "educations")
        {
        }

        public async Task<List<EducationDto>> GetAllAdminAsync()
        {
            var query = await _httpClient.GetFromJsonAsync<List<EducationDto>>($"{_endpoint}/admin-all");
            if (query == null)
            {
                return new List<EducationDto>();
            }
            return query;
        }

        public async Task RestoreAsync(Guid guid)
        {
            var response = await _httpClient.PutAsync($"{_endpoint}/restore/{guid}", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadAsStringAsync();
            }
        }
    }
}
