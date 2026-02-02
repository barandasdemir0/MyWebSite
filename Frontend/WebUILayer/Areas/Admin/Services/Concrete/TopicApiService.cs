using DtoLayer.TopicDto;
using Mapster;
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
                
            }
        }

        //public async Task<UpdateTopicDto> GetTopicForEditAsync()
        //{
        //    var list = await GetAllAsync();
        //    var query = list.FirstOrDefault();
        //    if (query == null)
        //    {
        //        return new UpdateTopicDto();
        //    }
        //    return query.Adapt<UpdateTopicDto>();
        //}

        //public async Task SaveTopicAsync(UpdateTopicDto updateTopicDto)
        //{
        //    var list = await GetAllAsync();
        //    var query = list.FirstOrDefault();
        //    if (query == null)
        //    {
        //        var create = updateTopicDto.Adapt<CreateTopicDto>();
        //        await AddAsync(create);
        //    }
        //    else
        //    {
        //        await UpdateAsync(query.Id, updateTopicDto);
        //    }
        //}
    }
}
