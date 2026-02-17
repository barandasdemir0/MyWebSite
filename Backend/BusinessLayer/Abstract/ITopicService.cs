using CV.EntityLayer.Entities;
using DtoLayer.TopicDtos;

namespace BusinessLayer.Abstract;

public interface ITopicService:IGenericService<Topic,TopicDto,CreateTopicDto,UpdateTopicDto>
{
    Task<TopicDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<List<TopicDto>> GetAllAdminAsync( CancellationToken cancellationToken = default);
}
