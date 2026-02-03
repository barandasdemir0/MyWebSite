using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface ITopicApiService:IGenericApiService<TopicDto,CreateTopicDto,UpdateTopicDto>
    {
        //Task<UpdateTopicDto> GetTopicForEditAsync();
        //Task SaveTopicAsync(UpdateTopicDto updateTopicDto);
        Task<List<TopicDto>> GetAllAdminAsync();
        Task RestoreAsync(Guid guid);
    }
}
