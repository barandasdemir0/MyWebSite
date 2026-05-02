using DtoLayer.TopicDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicTopicApiService : PublicReadApiService<TopicDto>, IPublicTopicApiService
{
    public PublicTopicApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "topics")
    {
    }
}
