using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface ITopicApiService:IGenericApiService<TopicDto,CreateTopicDto,UpdateTopicDto>
    {
     
        Task<List<TopicDto>> GetAllAdminAsync();
        Task RestoreAsync(Guid guid);
    }
}
