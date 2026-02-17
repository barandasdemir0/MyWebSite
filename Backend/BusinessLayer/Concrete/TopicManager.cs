using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.TopicDtos;
using MapsterMapper;

namespace BusinessLayer.Concrete;

public class TopicManager : GenericManager<Topic,TopicDto,CreateTopicDto,UpdateTopicDto> ,ITopicService
{

    private readonly ITopicDal _topicDal;

    public TopicManager(ITopicDal topicDal, IMapper mapper) : base(topicDal, mapper)
    {
        _topicDal = topicDal;
    }


    public async Task<List<TopicDto>> GetAllAdminAsync( CancellationToken cancellationToken = default)
    {
        var entity = await _topicDal.GetAllAdminAsync(tracking: false, cancellationToken: cancellationToken);
        return _mapper.Map<List<TopicDto>>(entity);
    }

    public async Task<TopicDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        var entity = await _topicDal.RestoreDeleteByIdAsync(guid, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return null;
        }
        entity.IsDeleted = false;
        entity.DeletedAt = null;

        await _topicDal.UpdateAsync(entity, cancellationToken: cancellationToken);
        await _topicDal.SaveAsync(cancellationToken);

        return _mapper.Map<TopicDto>(entity);
    }

}
