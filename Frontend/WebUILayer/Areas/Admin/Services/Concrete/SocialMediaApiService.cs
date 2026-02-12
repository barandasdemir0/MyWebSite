using DtoLayer.SocialMediaDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class SocialMediaApiService : GenericApiService<SocialMediaDto, CreateSocialMediaDto, UpdateSocialMediaDto>, ISocialMediaApiService
    {
        public SocialMediaApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "socialmedias")
        {
        }

        public async Task<List<SocialMediaDto>> GetAdminAllAsync()
        {
            var query = await _httpClient.GetFromJsonAsync<List<SocialMediaDto>>($"{_endpoint}/admin-all");
            if (query==null)
            {
                return new List<SocialMediaDto>();
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
