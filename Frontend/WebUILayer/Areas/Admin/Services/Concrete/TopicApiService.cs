using DtoLayer.TopicDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class TopicApiService : GenericApiService<TopicDto, CreateTopicDto, UpdateTopicDto>, ITopicApiService
    {
        public TopicApiService(HttpClient httpClient) : base(httpClient, "topics")
        {
        }

        public async Task<List<TopicDto>> GetAllAdminAsync()
        {
            var query = await _httpClient.GetFromJsonAsync<List<TopicDto>>($"{_endpoint}/admin-all");
            if (query == null)
            {
                return new List<TopicDto>();
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
